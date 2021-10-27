module.exports = {
  purge: [],
  darkMode: false, // or 'media' or 'class'
  theme: {
    screens: {
      '3xl': { 'min': '1750px' },
      // => @media (max-width: 1535px) { ... }

      '2xl': { 'min': '1540px' },
      // => @media (max-width: 1535px) { ... }

      'xl': { 'min': '1280px' },
      // => @media (max-width: 1279px) { ... }

      'lg': { 'min': '1024px' },
      // => @media (max-width: 1023px) { ... }

      'md': { 'min': '768px' },
      // => @media (max-width: 767px) { ... }

      'sm': { 'min': '640px' },
      // => @media (max-width: 639px) { ... }

      'xs': { 'min': '400px' },
      // => @media (max-width: 639px) { ... }
    },
    extend: {},
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
