
function data() {
    function getThemeFromLocalStorage() {
        // if user already changed the theme, use it
        if (window.localStorage.getItem('dark')) {
            return JSON.parse(window.localStorage.getItem('dark'))
        }

        // else return their preferences
        return (
            !!window.matchMedia &&
            window.matchMedia('(prefers-color-scheme: dark)').matches
        )
    }

    function setThemeToLocalStorage(value) {
        window.localStorage.setItem('dark', value)
    }

    return {
        dark: getThemeFromLocalStorage(),
        toggleTheme() {
            this.dark = !this.dark
            setThemeToLocalStorage(this.dark)
        },
        isDropdownOpen: false,
        toggleDropdown() {
            this.isDropdownOpen = !this.isDropdownOpen
        },
        closeDropdown() {
            this.isDropdownOpen = false
        },
        isProfileMenuOpen: false,
        toggleProfileMenu() {
            this.isProfileMenuOpen = !this.isProfileMenuOpen
        },
        closeProfileMenu() {
            this.isProfileMenuOpen = false
        },

        isJobMenuOpen: false,
        toggleJobMenu() {
            this.isJobMenuOpen = !this.isJobMenuOpen
        },
        closeJobMenu() {
            this.isJobMenuOpen = false
        },

        isPagesMenuOpen: false,
        togglePagesMenu() {
            this.isPagesMenuOpen = !this.isPagesMenuOpen
        },
        // Modal
        isModalOpen: false,
        trapCleanup: null,
        openModal() {
            this.isModalOpen = true
            this.trapCleanup = focusTrap(document.querySelector('#modal'))
        },
        closeModal() {
            this.isModalOpen = false
            this.trapCleanup()
        },
        
    }
}


window.setTimeout(() => {
    $("#dismiss-alert").fadeTo(300, 0).slideUp(300, function () {
        $(this).remove();
    });
}, 2000);


function getTwoLettersForName(name) {
    if (typeof name !== 'string' || name.length === 0) {
        return ''; // Return an empty string if the input is not valid
    }

    // Remove leading and trailing whitespace, if any
    name = name.trim();

    // Split the name into words
    const words = name.split(' ');

    // Extract the first letter of each word
    const letters = words.map(word => word.charAt(0));

    // Join the letters into a string
    const twoLetters = letters.join('');

    return twoLetters;
}

