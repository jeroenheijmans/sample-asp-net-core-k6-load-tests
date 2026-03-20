import { open } from 'k6/experimental/fs';
import csv from 'k6/experimental/csv';
import http from 'k6/http';
import { check } from 'k6';
import { scenario } from 'k6/execution';
import type { Options } from 'k6/options';
import { config } from './config.ts';

export const options: Options = {
  scenarios: {
    base: {
      executor: 'shared-iterations',
      vus: 2,
      iterations: 10,
    },
  },
};

const file = await open('./test-input/base-data.csv');
const csvRecords = (await csv.parse(file, { delimiter: ',' })) as unknown as string[][];
const csvDataLength = csvRecords.length - 1; // subtract header row

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

  const rowIndex = 1 + (scenario.iterationInTest % csvDataLength);
  const [colorSlug, subjectId] = csvRecords[rowIndex] as [string, string];

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

  const colorDetailResponse = http.get(http.url`${config.baseDomain}/colors/${colorSlug}`);
  check(colorDetailResponse, {
    'GET /colors/:colorSlug status 200': (r) => r.status === 200,
    'GET /colors/:colorSlug response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorSubjectsResponse = http.get(http.url`${config.baseDomain}/colors/${colorSlug}/subjects`);
  check(colorSubjectsResponse, {
    'GET /colors/:colorSlug/subjects status 200': (r) => r.status === 200,
    'GET /colors/:colorSlug/subjects response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorSubjectDetailResponse = http.get(http.url`${config.baseDomain}/colors/${colorSlug}/subjects/${subjectId}`);
  check(colorSubjectDetailResponse, {
    'GET /colors/:colorSlug/subjects/:subjectId status 200': (r) => r.status === 200,
    'GET /colors/:colorSlug/subjects/:subjectId response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const subjectsResponse = http.get(`${config.baseDomain}/subjects`);
  check(subjectsResponse, {
    'GET /subjects status 200': (r) => r.status === 200,
    'GET /subjects response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const subjectDetailResponse = http.get(http.url`${config.baseDomain}/subjects/${subjectId}`);
  check(subjectDetailResponse, {
    'GET /subjects/:subjectId status 200': (r) => r.status === 200,
    'GET /subjects/:subjectId response time <= 1000ms': (r) => r.timings.duration <= 1000,
  });

  const colorExtraDetailsResponse = http.get(http.url`${config.baseDomain}/colors/${colorSlug}/extra-details`);
  check(colorExtraDetailsResponse, {
    'GET /colors/:colorSlug/extra-details status 200': (r) => r.status === 200,
    'GET /colors/:colorSlug/extra-details response time <= 1000ms': (r) => r.timings.duration <= 1000,
    'GET /colors/:colorSlug/extra-details body contains "Extra Details"': (r) => typeof r.body === 'string' && r.body.includes('Extra Details'),
    'GET /colors/:colorSlug/extra-details body contains colorSlug': (r) => typeof r.body === 'string' && r.body.includes(colorSlug),
  });
}
