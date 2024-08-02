import { sentryVitePlugin } from "@sentry/vite-plugin";
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), sentryVitePlugin({
    org: "chandan-kumar-yg",
    project: "iphone-15-clone-website"
  }),],

  // server: {
  //   headers: {
  //     'Document-Policy': 'js-profiling',
  //   },
  // },

  build: {
    sourcemap: true
  }
})