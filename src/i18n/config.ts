import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import es from './locales/es.json';
import en from './locales/en.json';

export const defaultNS = 'translation';
export const resources = {
  es: { translation: es },
  en: { translation: en },
} as const;

// Read previously saved language preference or default to Spanish / browser language
const savedLang = localStorage.getItem('rd_app_language') || (navigator.language.startsWith('es') ? 'es' : 'es');

i18n
  .use(initReactI18next)
  .init({
    resources,
    lng: savedLang,
    fallbackLng: 'es',
    interpolation: {
      escapeValue: false, // React already escapes values
    },
    defaultNS,
  });

export default i18n;
