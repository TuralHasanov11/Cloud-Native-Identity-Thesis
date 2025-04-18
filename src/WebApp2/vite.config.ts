import { fileURLToPath, URL } from 'node:url'
import fs from 'fs'
import path from 'path'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'
import ui from '@nuxt/ui/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
    ui({
      autoImport: {
        dirs: [
          './src/components',
          './src/layouts',
          './src/pages',
          './src/plugins',
          './src/store',
          './src/utils',
          './src/composables',
        ],
        include: [
          /\.[tj]sx?$/, // .ts, .tsx, .js, .jsx
          /\.vue$/,
          /\.vue\?vue/, // .vue
          /\.vue\.[tj]sx?\?vue/, // .vue (vue-loader with experimentalInlineMatchResource enabled)
          /\.md$/, // .md
        ],
      },
      components: {
        dts: true,
        dirs: ['./src/components', './src/layout'],
        extensions: ['vue'],
        deep: true,
        directoryAsNamespace: false,
      },
    }),
  ],

  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  server: {
    // https: {
    //   key: fs.readFileSync(path.resolve(__dirname, 'cert/webapp-key.pem')),
    //   cert: fs.readFileSync(path.resolve(__dirname, 'cert/webapp.pem')),
    // },
    port: 5002,
  },
})
