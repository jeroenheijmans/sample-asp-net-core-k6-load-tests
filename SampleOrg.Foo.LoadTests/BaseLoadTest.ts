import http from 'k6/http';
import { check } from 'k6';
import type { Options } from 'k6/options';
import { config } from './config.ts';

export const options: Options = {
  scenarios: {
    base: {
      executor: 'shared-iterations',
      vus: 1,
      iterations: 1,
    },
  },
};

interface SetupData {
  cookies: Record<string, string>;
}

export function setup(): SetupData {
  // Confirm GET / redirects to login
  const redirectRes = http.get(`${config.baseDomain}/`, { redirects: 0 });
  check(redirectRes, {
    'GET / redirects to login (302)': (r) => r.status === 302,
  });

  // Confirm login page is reachable
  const loginPageRes = http.get(`${config.baseDomain}/user?returnUrl=/`);
  check(loginPageRes, {
    'GET /user?returnUrl=/ is 200': (r) => r.status === 200,
  });

  // Log in — server will set the auth cookie and redirect back to /
  const loginRes = http.post(
    `${config.baseDomain}/user`,
    `returnUrl=%2F&username=${encodeURIComponent(config.username)}&password=${encodeURIComponent(config.password)}`,
    { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
  );
  check(loginRes, {
    'POST /user login succeeds (200)': (r) => r.status === 200,
  });

  // Extract auth cookies to pass to VUs
  const rawCookies = http.cookieJar().cookiesForURL(`${config.baseDomain}/`);
  const cookies: Record<string, string> = {};
  for (const [name, values] of Object.entries(rawCookies)) {
    if (values.length > 0) {
      cookies[name] = values[0];
    }
  }

  return { cookies };
}

export default function (data: SetupData): void {
  // Restore auth cookies for this VU
  const jar = http.cookieJar();
  for (const [name, value] of Object.entries(data.cookies)) {
    jar.set(`${config.baseDomain}/`, name, value);
  }

  const homeRes = http.get(`${config.baseDomain}/`);
  check(homeRes, {
    'GET / status 200': (r) => r.status === 200,
    'GET / response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorsRes = http.get(`${config.baseDomain}/colors`);
  check(colorsRes, {
    'GET /colors status 200': (r) => r.status === 200,
    'GET /colors response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });
}
