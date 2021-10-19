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
    theme: {
      screens: {
        'xsm': '0px',
        'sm': '320px',
        'md': '640px',
        // => @media (min-width: 640px) { ... }
  
        'lg': '768px',
        // => @media (min-width: 768px) { ... }
  
        'xl': '1024px',
        // => @media (min-width: 1024px) { ... }
  
        '2xl': '1280px',
        // => @media (min-width: 1280px) { ... }
  
        '3xl': '1536px',
        // => @media (min-width: 1536px) { ... }
      }
    },
    extend: {},
  },
  variants: {
    extend: {},
  },
  plugins: [],
}
