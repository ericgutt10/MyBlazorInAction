module.exports = {
  mode: 'jit',
  purge: {
    enabled: false,
    content: [
      './**/*.razor',
      './**/*.cshtml'
    ]
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {},
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
