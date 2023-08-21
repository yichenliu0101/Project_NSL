document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        timeZone: 'UTC',
        initialView: 'timeTableOfWeek',
        headerToolbar: {
            left: 'prev,next',
            center: 'title',
            right: 'resourceTimeGridDay,timeTableOfWeek'
        },
        allDaySlot: false,
        views: {
            timeTableOfWeek: {
                type: 'resourceTimeGrid',
                duration: { days: 7 },
                buttonText: '7 days',
                slotDuration: { hours: 1 }, // 將每個時間段的持續時間設置為1小時
                firstDay: 0 // 將第一天設定為星期日
            },
        },
        resources: [
            { id: 'Course', title: '課表' }
        ],
        events: 'https://fullcalendar.io/api/demo-feeds/events.json?with-resources=2',
    });

    calendar.render();
});