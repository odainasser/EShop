/**
 * BitFlow Application Utilities
 * Unified JavaScript module for culture, theme, and app utilities
 * @version 1.0.0
 */

(function (window, document) {
    'use strict';

    // ===========================================
    // TAILWIND CONFIGURATION
    // ===========================================
    
    if (window.tailwind && window.tailwind.config) {
        window.tailwind.config = {
            theme: {
                extend: {
                    colors: {
                        purple: {
                            50: '#FBF7EE',
                            100: '#F7EEDC',
                            200: '#EFDDB9',
                            300: '#E6CF95',
                            400: '#DDBF78',
                            500: '#e2c675',
                            600: '#e2c675', // Primary brand
                            700: '#b89956',
                            800: '#8b6d3b',
                            900: '#5d4425',
                            950: '#2f2212',
                        }
                    }
                }
            }
        };
    }

    // ===========================================
    // COOKIE MANAGEMENT
    // ===========================================
    
    const Cookie = {
        get(name) {
            const value = `; ${document.cookie}`;
            const parts = value.split(`; ${name}=`);
            return parts.length === 2 ? parts.pop().split(';').shift() : null;
        },

        set(name, value, days = 365) {
            const expires = new Date();
            expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
            document.cookie = `${name}=${value}; expires=${expires.toUTCString()}; path=/; SameSite=Lax`;
        },

        remove(name) {
            document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
        }
    };

    // ===========================================
    // URL & QUERY PARAMETERS
    // ===========================================
    
    const URL = {
        getQueryParam(name) {
            const params = new URLSearchParams(window.location.search);
            return params.get(name);
        },

        getAllQueryParams() {
            const params = new URLSearchParams(window.location.search);
            const result = {};
            for (const [key, value] of params) {
                result[key] = value;
            }
            return result;
        },

        updateQueryParam(name, value) {
            const url = new window.URL(window.location);
            url.searchParams.set(name, value);
            window.history.pushState({}, '', url);
        }
    };

    // ===========================================
    // CULTURE & LOCALIZATION
    // ===========================================
    
    const Culture = {
        current: null,

        initialize() {
            let culture = URL.getQueryParam('culture') || URL.getQueryParam('ui-culture');

            if (!culture) {
                const cookie = Cookie.get('.AspNetCore.Culture');
                if (cookie) {
                    const match = cookie.match(/c=([^|;]+)/);
                    if (match) culture = match[1];
                }
            }

            if (!culture) {
                culture = navigator.language || navigator.userLanguage || 'en';
            }

            culture = culture.split('-')[0].toLowerCase();
            this.current = culture;

            // Apply to DOM
            document.documentElement.lang = culture;
            const dir = (culture === 'ar') ? 'rtl' : 'ltr';
            document.documentElement.dir = dir;
            if (document.body) document.body.dir = dir;

            return culture;
        },

        setCookie(culture) {
            try {
                const cookieValue = `c=${culture}|uic=${culture}`;
                const expires = new Date();
                expires.setFullYear(expires.getFullYear() + 1);
                document.cookie = `.AspNetCore.Culture=${cookieValue}; expires=${expires.toUTCString()}; path=/; SameSite=Lax`;
                window.location.reload();
            } catch (e) {
                console.error('Failed to set culture cookie:', e);
            }
        },

        isRTL() {
            return this.current === 'ar';
        }
    };

    // ===========================================
    // THEME MANAGEMENT
    // ===========================================
    
    const Theme = {
        // Color conversion utilities
        hexToRgb(hex) {
            hex = hex.trim().replace('#', '');
            if (hex.length === 3) {
                hex = hex.split('').map(c => c + c).join('');
            }
            if (hex.length !== 6) return null;

            return {
                r: parseInt(hex.substr(0, 2), 16),
                g: parseInt(hex.substr(2, 2), 16),
                b: parseInt(hex.substr(4, 2), 16)
            };
        },

        rgbToHex(r, g, b) {
            return `#${r.toString(16).padStart(2, '0')}${g.toString(16).padStart(2, '0')}${b.toString(16).padStart(2, '0')}`;
        },

        // Calculate relative luminance
        getLuminance(r, g, b) {
            return (0.299 * r + 0.587 * g + 0.114 * b) / 255;
        },

        // Get contrast color (black or white)
        getContrastColor(r, g, b) {
            const lum = this.getLuminance(r, g, b);
            return lum > 0.6 ? '#000000' : '#ffffff';
        },

        // Darken color
        darken(value, factor = 0.85) {
            return Math.max(0, Math.min(255, Math.round(value * factor)));
        },

        // Create tint (lighter version)
        createTint(r, g, b, percent) {
            const tintR = Math.round(r + (255 - r) * (1 - percent));
            const tintG = Math.round(g + (255 - g) * (1 - percent));
            const tintB = Math.round(b + (255 - b) * (1 - percent));
            return this.rgbToHex(tintR, tintG, tintB);
        },

        // Set theme colors
        setColors(/*primaryColor*/) {
            // Runtime theming disabled: theme colors are static in CSS.
            console.warn('Runtime theming disabled; using static CSS variables from Web.styles.css');
            return false;
        },

        // Get current primary color
        getCurrentColor() {
            return getComputedStyle(document.documentElement)
                .getPropertyValue('--bf-primary')
                .trim();
        }
    };

    // ===========================================
    // DOM UTILITIES
    // ===========================================
    
    const DOM = {
        ready(callback) {
            if (document.readyState !== 'loading') {
                callback();
            } else {
                document.addEventListener('DOMContentLoaded', callback);
            }
        },

        on(element, event, callback) {
            if (typeof element === 'string') {
                element = document.querySelector(element);
            }
            if (element) {
                element.addEventListener(event, callback);
            }
        },

        toggle(element, className) {
            if (typeof element === 'string') {
                element = document.querySelector(element);
            }
            if (element) {
                element.classList.toggle(className);
            }
        },

        hide(elementId) {
            const element = typeof elementId === 'string' 
                ? document.getElementById(elementId) 
                : elementId;
            if (element) {
                element.style.display = 'none';
            }
        },

        show(elementId) {
            const element = typeof elementId === 'string' 
                ? document.getElementById(elementId) 
                : elementId;
            if (element) {
                element.style.display = '';
            }
        }
    };

    // ===========================================
    // STORAGE (LocalStorage/SessionStorage)
    // ===========================================
    
    const Storage = {
        local: {
            get(key) {
                try {
                    const item = localStorage.getItem(key);
                    return item ? JSON.parse(item) : null;
                } catch (e) {
                    console.error('Storage.local.get error:', e);
                    return null;
                }
            },

            set(key, value) {
                try {
                    localStorage.setItem(key, JSON.stringify(value));
                    return true;
                } catch (e) {
                    console.error('Storage.local.set error:', e);
                    return false;
                }
            },

            remove(key) {
                try {
                    localStorage.removeItem(key);
                    return true;
                } catch (e) {
                    console.error('Storage.local.remove error:', e);
                    return false;
                }
            },

            clear() {
                try {
                    localStorage.clear();
                    return true;
                } catch (e) {
                    console.error('Storage.local.clear error:', e);
                    return false;
                }
            }
        },

        session: {
            get(key) {
                try {
                    const item = sessionStorage.getItem(key);
                    return item ? JSON.parse(item) : null;
                } catch (e) {
                    console.error('Storage.session.get error:', e);
                    return null;
                }
            },

            set(key, value) {
                try {
                    sessionStorage.setItem(key, JSON.stringify(value));
                    return true;
                } catch (e) {
                    console.error('Storage.session.set error:', e);
                    return false;
                }
            },

            remove(key) {
                try {
                    sessionStorage.removeItem(key);
                    return true;
                } catch (e) {
                    console.error('Storage.session.remove error:', e);
                    return false;
                }
            },

            clear() {
                try {
                    sessionStorage.clear();
                    return true;
                } catch (e) {
                    console.error('Storage.session.clear error:', e);
                    return false;
                }
            }
        }
    };

    // ===========================================
    // BLAZOR ERROR UI HANDLER
    // ===========================================
    
    const ErrorUI = {
        hide() {
            DOM.hide('blazor-error-ui');
        },

        show() {
            DOM.show('blazor-error-ui');
        }
    };

    // ===========================================
    // INITIALIZATION
    // ===========================================
    
    // Auto-initialize culture on load
    Culture.initialize();

    // Setup error UI close handler
    DOM.ready(() => {
        const errorUI = document.getElementById('blazor-error-ui');
        if (errorUI) {
            const closeButton = errorUI.querySelector('button');
            if (closeButton) {
                closeButton.onclick = () => ErrorUI.hide();
            }
        }
    });

    // ===========================================
    // PUBLIC API
    // ===========================================
    
    const App = {
        Cookie,
        URL,
        Culture,
        Theme,
        DOM,
        Storage,
        ErrorUI,
        version: '1.0.0'
    };

    // Export to window
    window.App = App;

    // Backward compatibility aliases
    window.setCultureCookie = Culture.setCookie.bind(Culture);
    window.AppUtils = {
        getCookie: Cookie.get,
        setCookie: Cookie.set,
        getQueryParam: URL.getQueryParam,
        setCultureCookie: Culture.setCookie.bind(Culture),
        initializeCulture: Culture.initialize.bind(Culture)
    };

    // Log initialization
    console.log(`%c? App v${App.version} loaded`, 'color: #e2c675; font-weight: bold;');
    console.log(`  Culture: ${Culture.current} | RTL: ${Culture.isRTL()}`);

})(window, document);
