/**
 * Copyright (c) 2021 Peyton Gasink
 * Distributed under MIT License.
 *
 * This file contains all the necessary scripting to programmatically
 * generate Bootstrap toasts. It first inserts a container at the bottom
 * of the DOM, then fills a toast template and inserts it into the container.
 *
 * Configuration options are also provided for toast placement, light & dark themes,
 * and the maximum number of toasts allowed on the page at a given time.
 * 
 * significantly modified by
 * Paulo Almeida Araújo
 * Original version does not work in IE
 * Some changes were made to declutter the namespace and support Internet explorer
 * Runtime changes to the TOAST_CONTAINER were required to work properlly
 * Scrool to ensure the most recent toast is visible
 * When a new toast is added and the limit of toasts is reached, the oldest toast is removed 
 * A default instance (Toast) of the class UserMessage is created at the end of the document
 * Aditional instances of this class may be used, provided they have distinct TOAST_PLACEMENT
 */


/** Emulates enum functionality for setting toast statuses without needing to remember actual values. */
const TOAST_STATUS = {
    SUCCESS: 1,
    DANGER: 2,
    WARNING: 3,
    INFO: 4
};
/** Emulates enum functionality for setting toast container placement. */
const TOAST_PLACEMENT = {
    TOP_LEFT: 1,
    TOP_CENTER: 2,
    TOP_RIGHT: 3,
    MIDDLE_LEFT: 4,
    MIDDLE_CENTER: 5,
    MIDDLE_RIGHT: 6,
    BOTTOM_LEFT: 7,
    BOTTOM_CENTER: 8,
    BOTTOM_RIGHT: 9
};
/** Emulates enum functionality for setting toast themes. */
const TOAST_THEME = {
    LIGHT: 1,
    DARK: 2
};


var UserMessage = function (options) {

    /* ******************************************
     * private members */

    /** Container that generated toasts will be inserted into. */
    const TOAST_CONTAINER = document.createElement("div");
    TOAST_CONTAINER.id = "toastContainer" + Math.random().toString().substr(2, 8);
    TOAST_CONTAINER.className = "toastContainer position-fixed toast-bottom-0 toast-right-0";
    TOAST_CONTAINER.setAttribute("aria-live", "polite");
    document.body.appendChild(TOAST_CONTAINER);

    /** HTML markup for the toast template. */
    const TOAST_TEMPLATE = document.createElement("div");
    TOAST_TEMPLATE.className = "toast";
    TOAST_TEMPLATE.classList.add("toast-theme-dark");
    TOAST_TEMPLATE.setAttribute("role", "status");
    TOAST_TEMPLATE.setAttribute("aria-live", "polite");
    TOAST_TEMPLATE.setAttribute("aria-atomic", "true");
    TOAST_TEMPLATE.setAttribute("data-bs-autohide", "false");
    TOAST_TEMPLATE.innerHTML = '<div class="toast-header toast-theme-dark">'
        + '<span class="status-icon bi me-2" aria-hidden="true"></span>'
        + '<strong class="me-auto toast-title"></strong>'
        + '<small class="timer" aria-hidden="true"></small>'
        + '<button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>'
        + '</div>'
        + '<div class="toast-body"></div>';

    var maxToastCountOption = 2000;/** Maximum amount of toasts to be allowed on the page at once. */
    var enableTimersOption = true;/** Controls whether elapsed time will be displayed in the toast header. */
    var currentToastCount = 0; /** Number of toasts currently rendered on the page. */
    var that = this;

    function _setStatus(toast, status) {
        let statusIcon = toast.querySelector(".status-icon");

        switch (status) {
            case TOAST_STATUS.SUCCESS:
                statusIcon.classList.add("text-success");
                statusIcon.classList.add("bi-check-circle-fill");
                break;
            case TOAST_STATUS.DANGER:
                statusIcon.classList.add("text-danger" );
                statusIcon.classList.add( "bi-x-circle-fill");
                toast.setAttribute("role", "alert");
                toast.setAttribute("aria-live", "assertive");
                break;
            case TOAST_STATUS.WARNING:
                statusIcon.classList.add("text-warning");
                statusIcon.classList.add("bi-exclamation-circle-fill");
                toast.setAttribute("role", "alert");
                toast.setAttribute("aria-live", "assertive");
                break;
            case TOAST_STATUS.INFO:
                statusIcon.classList.add("text-info" );
                statusIcon.classList.add( "bi-info-circle-fill");
                break;
            default:
                statusIcon.classList.add("d-none");
                break;
        }
    }

    function _render(toast, timeout) {
        if (timeout > 0) {
            toast.setAttribute("data-bs-delay", timeout);
            toast.setAttribute("data-bs-autohide", true);
        }

        let timer = toast.querySelector(".timer");

        if (enableTimersOption) {
            // Start a timer that updates the text of the time indicator every minute
            // Initially set to 1 because for the first minute the indicator reads "just now"
            let minutes = 1
            let elapsedTimer = setInterval(function () {
                timer.innerText = ' há ' + minutes + ' min';
                minutes++;
            }, 60 * 1000);

            // When the toast hides, delete its timer instance
            $(toast).on('hidden.bs.toast', function () {
                clearInterval(elapsedTimer);
            });
        }
        else {
            let toastHeader = toast.querySelector(".toast-header");
            toastHeader.removeChild(timer);
        }

        TOAST_CONTAINER.appendChild(toast);
        $(toast).toast('show');
        currentToastCount++;

        var d = $('#' + TOAST_CONTAINER.id);
        d.scrollTop(d.prop("scrollHeight"));

        // When the toast hides, remove it from the DOM
        $(toast).on('hidden.bs.toast', function () {
            TOAST_CONTAINER.removeChild(toast);
            currentToastCount--;
        });
    }

    this.setMaxCount = function (maxToasts) {
        if (maxToasts !== null && maxToasts > 0) {
                maxToastCountOption = maxToasts;
        }
    }

    this.setPlacement = function (placement) {
        
        switch (placement) {
            case TOAST_PLACEMENT.TOP_LEFT:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-top-0 toast-left-0";
                break;
            case TOAST_PLACEMENT.TOP_CENTER:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-top-0 toast-left-50 toast-translate-middle-x";
                break;
            case TOAST_PLACEMENT.TOP_RIGHT:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-top-0 toast-right-0";
                break;
            case TOAST_PLACEMENT.MIDDLE_LEFT:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-top-50 toast-left-0 toast-translate-middle-y";
                break;
            case TOAST_PLACEMENT.MIDDLE_CENTER:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-top-50 toast-left-50 toast-translate-middle";
                break;
            case TOAST_PLACEMENT.MIDDLE_RIGHT:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-top-50 toast-right-0 toast-translate-middle-y";
                break;
            case TOAST_PLACEMENT.BOTTOM_LEFT:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-bottom-0 toast-left-0";
                break;
            case TOAST_PLACEMENT.BOTTOM_CENTER:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-bottom-0 toast-left-50 toast-translate-middle-x";
                break;
            case TOAST_PLACEMENT.BOTTOM_RIGHT:
            default:
                TOAST_CONTAINER.className = "toastContainer position-fixed toast-bottom-0 toast-right-0";
                break;
        }
    }

    this.setTheme = function (theme) {
        let header = TOAST_TEMPLATE.querySelector(".toast-header");
        let close = header.querySelector(".close");

        TOAST_TEMPLATE.classList.remove("toast-theme-dark");
        header.classList.remove("toast-theme-dark");
        close.classList.remove("toast-theme-dark");
        TOAST_TEMPLATE.classList.remove("toast-theme-light");
        header.classList.remove("toast-theme-light");
        close.classList.remove("toast-theme-light");

        switch (theme) {
            case TOAST_THEME.LIGHT:
                TOAST_TEMPLATE.classList.add("toast-theme-light");
                header.classList.add("toast-theme-light");
                close.classList.add("toast-theme-light");
                break;
            case TOAST_THEME.DARK:
                TOAST_TEMPLATE.classList.add("toast-theme-dark");
                header.classList.add("toast-theme-dark");
                close.classList.add("toast-theme-dark");
                break;
            default:
                break;
        }
    }

    this.enableTimers = function (enabled) {
        enableTimersOption = enabled;
    }

    this.configure = function (maxToasts, placement, theme, enableTimers) {
        if (theme === undefined) theme = TOAST_THEME.DARK;
        if (enableTimers === undefined) enableTimers = true;
        that.setMaxCount(maxToasts);
        that.setPlacement(placement);
        that.setTheme(theme);
        that.enableTimers(enableTimers);
    }

    this.create = function (title, message, status, timeout) {

        if (status === undefined) status = TOAST_STATUS.INFO;
        if (timeout === undefined) timeout = 3000;
        if (currentToastCount >= maxToastCountOption) {
            /* remove oldest toast*/
            $("#" + TOAST_CONTAINER.id).children(".toast").first().remove();
            currentToastCount--;
        }
            
        let toast = TOAST_TEMPLATE.cloneNode(true);
        let toastTitle = toast.querySelector(".toast-title");
        toastTitle.innerText = title;

        let toastBody = toast.querySelector(".toast-body");
        toastBody.innerHTML = message;

        _setStatus(toast, status);
        _render(toast, timeout);
    }
}

/* default instance to be used with bottom right placement and dark theme*/
var Toast = new UserMessage({});

