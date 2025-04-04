// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-11-01",
  devtools: { enabled: true },
  runtimeConfig:{
    bffProxyUrl: ''
  },
  modules: [
    "@nuxtjs/seo",
    // "@nuxt/content",
    "@nuxt/eslint",
    "@nuxt/fonts",
    "@nuxt/icon",
    "@nuxt/image",
    "@nuxt/scripts",
    "@nuxt/test-utils",
    "@nuxt/ui-pro",
    "@pinia/nuxt",
    "@nuxtjs/i18n",
    "@vueuse/nuxt",
    "nuxt-security",
    "nuxt-auth-utils",
  ],
  devServer:{
    https: {
      key: "./cert/webapp-key.pem",
      cert: "./cert/webapp.pem",
    },
  },
  vite:{
    server: {
      allowedHosts: ['webapp'],
    },
  },
  css: ["~/assets/css/main.css"],
  components: [{ path: "./components", pathPrefix: false }],
  i18n: {
    locales: [
      { code: "en", language: "en-US", name: "English", file: "en-US.json" },
      { code: "de", language: "de-DE", name: "Deutsch", file: "de-DE.json" },
    ],
    bundle: {
      optimizeTranslationDirective: false,
    },
    lazy: true,
    defaultLocale: "en",
    strategy: "no_prefix",
    langDir: "locales",
    restructureDir: false,
  },
  security: {
    strict: false,
    headers: {
      // crossOriginResourcePolicy: 'same-origin',
      crossOriginResourcePolicy: false,
      // crossOriginOpenerPolicy: 'same-origin-allow-popups',
      crossOriginOpenerPolicy: false,
      // crossOriginEmbedderPolicy: 'credentialless',
      crossOriginEmbedderPolicy: false,
      contentSecurityPolicy: {
        "base-uri": ["'none'"],
        "font-src": ["'self'", "https:", "data:"],
        "form-action": ["'self'"],
        "frame-ancestors": ["'self'"],
        "img-src": [
          "'self'",
          "data:",
          "https://github.com",
          "https://avatars.githubusercontent.com",
        ],
        "object-src": ["'none'"],
        "script-src-attr": ["'unsafe-inline'"],
        "style-src": ["'self'", "https:", "'unsafe-inline'"],
        "script-src": [
          "'self'",
          "https:",
          "'unsafe-inline'",
          "'strict-dynamic'",
          "'nonce-{{nonce}}'",
        ],
        "upgrade-insecure-requests": true,
      },
      originAgentCluster: "?1",
      referrerPolicy: "no-referrer",
      strictTransportSecurity: {
        maxAge: 15552000,
        includeSubdomains: true,
      },
      xContentTypeOptions: "nosniff",
      xDNSPrefetchControl: "off",
      xDownloadOptions: "noopen",
      xFrameOptions: "SAMEORIGIN",
      xPermittedCrossDomainPolicies: "none",
      xXSSProtection: "0",
      permissionsPolicy: {
        camera: [],
        "display-capture": [],
        fullscreen: [],
        geolocation: [],
        microphone: [],
      },
    },
    requestSizeLimiter: {
      maxRequestSizeInBytes: 2000000,
      maxUploadFileRequestInBytes: 8000000,
      throwError: true,
    },
    rateLimiter: {
      tokensPerInterval: 150,
      interval: 300000,
      headers: false,
      driver: {
        name: "lruCache",
      },
      throwError: true,
    },
    xssValidator: {
      throwError: true,
    },
    enabled: true,
    nonce: true,
  },
});
