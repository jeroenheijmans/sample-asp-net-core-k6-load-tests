interface Config {
  baseDomain: string;
  username: string;
  password: string;
}

const defaults: Config = {
  baseDomain: 'https://localhost:7177',
  username: 'bart',
  password: '', // intentionally empty — override this in config.local.json
};

let localOverrides: Partial<Config> = {};
try {
  localOverrides = JSON.parse(open('./config.local.json')) as Partial<Config>;
} catch {
  // no local config, defaults will be used
}

export const config: Config = { ...defaults, ...localOverrides };

if (!config.password) {
  throw new Error(
    'config.password is not set. Create a config.local.json file with { "password": "..." }.'
  );
}
