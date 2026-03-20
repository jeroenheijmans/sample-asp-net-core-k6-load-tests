import http from 'k6/http';
import { check } from 'k6';
import type { Options } from 'k6/options';
import { config } from './config.ts';

export const options: Options = {
  scenarios: {
    base: {
      executor: 'shared-iterations',
      vus: 2,
      iterations: 5,
    },
  },
};

interface SetupData {
  cookies: Record<string, string>;
}

export function setup(): SetupData {
  const redirectResponse = http.get(`${config.baseDomain}/`, { redirects: 0 });
  check(redirectResponse, {
    'GET / redirects to login (302)': (r) => r.status === 302,
  });

  const loginPageResponse = http.get(`${config.baseDomain}/user?returnUrl=/`);
  check(loginPageResponse, {
    'GET /user?returnUrl=/ is 200': (r) => r.status === 200,
  });

  const loginResponse = http.post(
    `${config.baseDomain}/user`,
    `returnUrl=%2F&username=${encodeURIComponent(config.username)}&password=${encodeURIComponent(config.password)}`,
    { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
  );
  check(loginResponse, {
    'POST /user login succeeds (200)': (r) => r.status === 200,
  });

  const cookiesForUrl = http.cookieJar().cookiesForURL(`${config.baseDomain}/`);
  const cookies: Record<string, string> = {};
  for (const [name, values] of Object.entries(cookiesForUrl)) {
    if (values.length > 0) {
      cookies[name] = values[0];
    }
  }

  return { cookies };
}

export default function (data: SetupData): void {
  const cookieJar = http.cookieJar();
  for (const [name, value] of Object.entries(data.cookies)) {
    cookieJar.set(`${config.baseDomain}/`, name, value);
  }

  const homeResponse = http.get(`${config.baseDomain}/`);
  check(homeResponse, {
    'GET / status 200': (r) => r.status === 200,
    'GET / response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorsResponse = http.get(`${config.baseDomain}/colors`);
  check(colorsResponse, {
    'GET /colors status 200': (r) => r.status === 200,
    'GET /colors response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorDetailResponse = http.get(`${config.baseDomain}/colors/red`);
  check(colorDetailResponse, {
    'GET /colors/red status 200': (r) => r.status === 200,
    'GET /colors/red response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorSubjectsResponse = http.get(`${config.baseDomain}/colors/red/subjects`);
  check(colorSubjectsResponse, {
    'GET /colors/red/subjects status 200': (r) => r.status === 200,
    'GET /colors/red/subjects response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorSubjectDetailResponse = http.get(`${config.baseDomain}/colors/red/subjects/1`);
  check(colorSubjectDetailResponse, {
    'GET /colors/red/subjects/1 status 200': (r) => r.status === 200,
    'GET /colors/red/subjects/1 response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const subjectsResponse = http.get(`${config.baseDomain}/subjects`);
  check(subjectsResponse, {
    'GET /subjects status 200': (r) => r.status === 200,
    'GET /subjects response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const subjectDetailResponse = http.get(`${config.baseDomain}/subjects/1`);
  check(subjectDetailResponse, {
    'GET /subjects/1 status 200': (r) => r.status === 200,
    'GET /subjects/1 response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });
}
