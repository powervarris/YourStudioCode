var Calendar = function (t) {
    this.divId = t.RenderID ? t.RenderID : '[data-render="calendar"]', this.DaysOfWeek = t.DaysOfWeek ? t.DaysOfWeek : ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], this.Months = t.Months ? t.Months : ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var e = new Date;
    this.CurrentMonth = e.getMonth(), this.CurrentYear = e.getFullYear();
    var r = t.Format;
    this.f = "string" == typeof r ? r.charAt(0).toUpperCase() : "M"
};
Calendar.prototype.nextMonth = function () {
    11 == this.CurrentMonth ? (this.CurrentMonth = 0, this.CurrentYear = this.CurrentYear + 1) : this.CurrentMonth = this.CurrentMonth + 1, this.divId = '[data-active="false"] .render', this.showCurrent()
}, Calendar.prototype.prevMonth = function () {
    0 == this.CurrentMonth ? (this.CurrentMonth = 11, this.CurrentYear = this.CurrentYear - 1) : this.CurrentMonth = this.CurrentMonth - 1, this.divId = '[data-active="false"] .render', this.showCurrent()
}, Calendar.prototype.previousYear = function () {
    this.CurrentYear = this.CurrentYear - 1, this.showCurrent()
}, Calendar.prototype.nextYear = function () {
    this.CurrentYear = this.CurrentYear + 1, this.showCurrent()
}, Calendar.prototype.showCurrent = function () {
    this.Calendar(this.CurrentYear, this.CurrentMonth)
}, Calendar.prototype.checkActive = function () {
    1 == document.querySelector(".months").getAttribute("class").includes("active") ? document.querySelector(".months").setAttribute("class", "months") : document.querySelector(".months").setAttribute("class", "months active"), "true" == document.querySelector(".month-a").getAttribute("data-active") ? (document.querySelector(".month-a").setAttribute("data-active", !1), document.querySelector(".month-b").setAttribute("data-active", !0)) : (document.querySelector(".month-a").setAttribute("data-active", !0), document.querySelector(".month-b").setAttribute("data-active", !1)), setTimeout(function () {
        document.querySelector(".calendar .header").setAttribute("class", "header active")
    }, 200), document.querySelector("body").setAttribute("data-theme", this.Months[document.querySelector('[data-active="true"] .render').getAttribute("data-month")].toLowerCase())
}, Calendar.prototype.Calendar = function (t, e) {
    "number" == typeof t && (this.CurrentYear = t), "number" == typeof t && (this.CurrentMonth = e);
    var r = (new Date).getDate(),
        n = (new Date).getMonth(),
        a = (new Date).getFullYear(),
        o = new Date(t, e, 1).getDay(),
        i = new Date(t, e + 1, 0).getDate(),
        u = 0 == e ? new Date(t - 1, 11, 0).getDate() : new Date(t, e, 0).getDate(),
        s = "<span>" + this.Months[e] + " &nbsp; " + t + "</span>",
        d = '<div class="table">';
    d += '<div class="row head">';
    for (var c = 0; c < 7; c++) d += '<div class="cell">' + this.DaysOfWeek[c] + "</div>";
    d += "</div>";
    for (var h, l = dm = "M" == this.f ? 1 : 0 == o ? -5 : 2, v = (c = 0, 0); v < 6; v++) {
        d += '<div class="row">';
        for (var m = 0; m < 7; m++) {
            if ((h = c + dm - o) < 1)
                d += '<div class="cell disable past">' + (u - o + l++) + "</div>";
            else if (h > i)
                d += '<div class="cell disable future">' + l++ + "</div>";
            else {
                var currentDate = new Date(this.CurrentYear, this.CurrentMonth, h);
                var today = new Date();
                if (currentDate < today && !isSameDay(currentDate, today)) {
                    d += '<div class="cell disable past"><span>' + h + "</span></div>";
                } else {
                    d += '<div class="cell' + (r == h && this.CurrentMonth == n && this.CurrentYear == a ? " active" : "") + '"><span data-date="' + h + '">' + h + "</span></div>", l = 1;
                }
            }
            c % 7 == 6 && h >= i && (v = 10), c++;
        }
        document.querySelector('.render-a').addEventListener('mousedown', function (event) {
            var target = event.target;
            if (target.classList.contains('cell') || target.tagName === 'SPAN') {
                var cell = target.closest('.cell');
                document.querySelectorAll('.cell').forEach(function (cell) {
                    cell.classList.remove('active');
                });
                cell.classList.add('active');
            }
        });


        function isSameDay(date1, date2) {
            return date1.getFullYear() === date2.getFullYear() && date1.getMonth() === date2.getMonth() && date1.getDate() === date2.getDate();
        }

        d += "</div>"
    }
    d += "</div>", document.querySelector('[data-render="month-year"]').innerHTML = s, document.querySelector(this.divId).innerHTML = d, document.querySelector(this.divId).setAttribute("data-date", this.Months[e] + " - " + t), document.querySelector(this.divId).setAttribute("data-month", e)
    }, Calendar.prototype.addCellClickListener = function () {
        var self = this; // Preserve reference to the Calendar object
        document.querySelector(this.divId).addEventListener('mousedown', function (event) {
            var target = event.target;
            if (target.classList.contains('cell') || target.tagName === 'SPAN') {
                var cell = target.closest('.cell');
                document.querySelectorAll('.cell').forEach(function (cell) {
                    cell.classList.remove('active');
                });
                cell.classList.add('active');
                // Optionally, you can access the selected date here using:
                // var selectedDate = cell.querySelector('span').getAttribute('data-date');
            }
        });
    };


window.onload = function () {
    var t = new Calendar({
        RenderID: ".render-a",
        Format: "M"
    });
    t.showCurrent();
    t.checkActive();
    t.addCellClickListener(); // Add cell click event listener

    var e = document.querySelectorAll(".header [data-action]");
    for (i = 0; i < e.length; i++) {
        e[i].onclick = function () {
            if (document.querySelector(".calendar .header").getAttribute("class") === "header") {
                document.querySelector(".calendar .header").setAttribute("class", "header");
            }
            if (document.querySelector(".months").getAttribute("data-loading") === "true") {
                document.querySelector(".calendar .header").setAttribute("class", "header active");
                return false;
            }
            var e;
            document.querySelector(".months").setAttribute("data-loading", "true");
            if (this.getAttribute("data-action").includes("prev")) {
                t.prevMonth();
                e = "left";
            } else {
                t.nextMonth();
                e = "right";
            }
            t.checkActive();
            document.querySelector(".months").setAttribute("data-flow", e);
            document.querySelector('.month[data-active="true"]').addEventListener("webkitTransitionEnd", function () {
                document.querySelector(".months").removeAttribute("data-loading");
            });
            document.querySelector('.month[data-active="true"]').addEventListener("transitionend", function () {
                document.querySelector(".months").removeAttribute("data-loading");
            });
            t.addCellClickListener(); // Add cell click event listener after rendering the new month
        };
    }
};