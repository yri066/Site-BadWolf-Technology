document.addEventListener('DOMContentLoaded', function () {
    const button = document.getElementById('popover-button');
    const popover = document.getElementById('sort-popover');

    button.addEventListener('click', function () {

        // Переключить видимость всплывающего окна.
        if (popover.style.display === 'none' || popover.style.display === '') {

            // Рассчитать положение.
            const rect = button.getBoundingClientRect();
            const popoverHeight = popover.offsetHeight;
            const triangleHeight = 10; // Высота треугольника.

            popover.style.display = 'block';
            popover.style.top = rect.bottom + window.scrollY + triangleHeight + 'px';
            popover.style.left = rect.left + window.scrollX + rect.width / 2 - popover.offsetWidth / 2 + 'px';
        } else {
            popover.style.display = 'none';
        }
    });

    // Закрытие всплывающего окна при нажатии за его пределами.
    document.addEventListener('click', function (event) {
        if (!button.contains(event.target) && !popover.contains(event.target)) {
            popover.style.display = 'none';
        }
    });
});
$(function () {
    // Прикрепление средства выбора дат и его настройка.
    $('button[name="datefilter"]').daterangepicker({
        autoUpdateInput: false,
        showDropdowns: true,
        ranges: {
            'За все время': [0, 0],
            'Сегодня': [moment(), moment()],
            'Вчера': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'За неделю': [moment().subtract(6, 'days'), moment()],
            'За месяц': [moment().subtract(1, 'month'), moment()],
            'За год': [moment().subtract(1, 'year'), moment()],
        },
        locale: {
            cancelLabel: 'Очистить',
            applyLabel: 'Выбрать',
            customRangeLabel: "За период...",
            firstDay: 1,
            daysOfWeek: [
                "Вс",
                "Пн",
                "Вт",
                "Ср",
                "Чт",
                "Пт",
                "Сб"
            ],
            monthNames: [
                'Январь',
                'Февраль',
                'Март',
                'Апрель',
                'Май',
                'Июнь',
                'Июль',
                'Август',
                'Сентябрь',
                'Октябрь',
                'Ноябрь',
                'Декабрь'
            ],
        }
    }).on('show.daterangepicker', function (ev, picker) {
        picker.container.addClass('daterangepicker-custom-css');
    });

    // Обработка выбора даты.
    $('button[name="datefilter"]').on('apply.daterangepicker', function (ev, picker) {

        if (picker.startDate.format('DD-MM-YYYY') == picker.endDate.format('DD-MM-YYYY') && picker.startDate.format('DD-MM-YYYY') != '01-01-1970') {
            $(this).text(picker.startDate.format('DD-MM-YYYY'))
            document.getElementById('startDate').value = picker.startDate.format('MM-DD-YYYY');
            document.getElementById('endDate').value = picker.endDate.format('MM-DD-YYYY');
        }
        else if (picker.startDate.format('DD-MM-YYYY') == '01-01-1970') {
            $(this).text("За все время");
            document.getElementById('startDate').value = null;
            document.getElementById('startDate').disabled = true
            document.getElementById('endDate').value = null;
            document.getElementById('endDate').disabled = true
        }
        else {
            $(this).text(picker.startDate.format('DD-MM-YYYY') + ' - ' + picker.endDate.format('DD-MM-YYYY'))
            document.getElementById('startDate').value = picker.startDate.format('MM-DD-YYYY');
            document.getElementById('endDate').value = picker.endDate.format('MM-DD-YYYY');
        }

        updateTempInputsForm();
        this.form.submit();
    });

    // Обработка отмены выбора даты.
    $('button[name="datefilter"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).text("За все время");
        document.getElementById('startDate').value = null;
        document.getElementById('startDate').disabled = true
        document.getElementById('endDate').value = null;
        document.getElementById('endDate').disabled = true

        updateTempInputsForm();
        this.form.submit();
    });
});

function updateTempInputsForm() {
    let sort = document.getElementById("dateSortForm").elements["SortOrder"];
    let seatch = document.getElementById('SearchString');

    if (sort.value == null || sort.value == "") {
        sort.disabled = true;
    }

    if (seatch.value == null || seatch.value == "") {
        seatch.disabled = true;
    }
}