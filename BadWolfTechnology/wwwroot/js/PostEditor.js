/* Конструктор макета страницы. */

document.addEventListener("DOMContentLoaded", function() {
    updateNumbering();
    initEditing();
});


/**
 * Установить превью при выборе изображения.
 * @param {Element} input Поле выбора файла.
 * @param {string} id Ид превью изображения.
 */
function updateItemPreview(input, id) {
    let file = input.files[0];
    let reader = new FileReader();

    reader.readAsDataURL(file);
    reader.onload = function () {
        let img = document.getElementById(id);
        img.src = reader.result;
    }
}

/**
 * Параметры редактора текста.
 */
const toolbarOptionsPostEditor = [
    ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
    ['blockquote', 'code-block'],
    ['link', 'image', 'formula'],

    [{ 'header': 1 }, { 'header': 2 }],               // custom button values
    [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'list': 'check' }],
    [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
    [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
    [{ 'direction': 'rtl' }],                         // text direction

    [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

    [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
    [{ 'font': [] }],
    [{ 'align': [] }],

    ['clean']                                         // remove formatting button
];

/**
 * Обновляет поле ввода при редактировании текста.
 * @param {string} editorId Ид текстового редактора.
 * @param {string} inputId Ид поля ввода.
 */
function updateItemEditor(editorId, inputId) {
    var divContent = document.getElementById(editorId).getElementsByClassName("ql-editor")[0].innerHTML;

    if (divContent == "<p><br></p>") {
        document.getElementsByName(inputId)[0].value = null;
    }
    else {
        document.getElementsByName(inputId)[0].value = divContent;
    }
}

/**
 * Инициализирует текстовый редактор в контейнере.
 * @param {string} id Ид текстового редактора.
 * @returns Объект текстового редактора.
 */
function setEditor(id) {
    return new Quill(id, {
        modules: {
            toolbar: toolbarOptionsPostEditor
        },
        theme: 'snow'
    });
}

/**
 * Контейнер в котором расположены блоки с различным контентом.
 */
const contentBlocks = document.getElementById("contents");

/**
 * Содержимое выпадающего списка с заголовками и стилями для элементов в блоке.
 */
const selectArray = [
    { "name": "Текст", "items": [
        { "name": "Title", "title": "Заголовок", "display": "block" },
        { "name": "Text", "title": "Текст", "display": "block" },
        { "name": "ImageName", "title": "Изображение", "display": "none" }] },
    { "name": "Изображение", "items": [
        { "name": "Title", "title": "Заголовок", "display": "none" },
        { "name": "Text", "title": "Текст", "display": "none" },
            { "name": "ImageName", "title": "Изображение", "display": "block" }]
    },
    { "name": "Слайдер", "items": [
        { "name": "Title", "title": "Заголовок", "display": "none" },
        { "name": "Text", "title": "Текст", "display": "none" },
        { "name": "ImageName", "title": "Изображение", "display": "block" }] },
    { "name": "Слайдер с текстом", "items": [
        { "name": "Title", "title": "Заголовок", "display": "none" },
        { "name": "Text", "title": "Текст", "display": "block" },
        { "name": "ImageName", "title": "Изображение", "display": "block" }] },
    { "name": "YouTube", "items": [
        { "name": "Title", "title": "Ссылка", "display": "block" },
        { "name": "Text", "title": "Текст", "display": "none" },
        { "name": "ImageName", "title": "Изображение", "display": "none" }] }
];

/**
 * Устанавливает обработчики событий.
 */
function initEditing() {
    const blocksItems = document.querySelectorAll('.item');
    if (blocksItems.length == 0) {
        return;
    }

    // Пройти циклом по блокам.
    blocksItems.forEach((block, blockIndex) => {
        block.dataset.blockNumber = blockIndex;

        // Выпадающий список.
        block.querySelector('select.item-select').onchange = function () {
            updateSubItemFields(block.querySelectorAll('.sub-item'), block.querySelector('select.item-select').value);
        };

        // Кнопка добавить элемент.
        block.querySelector('button.btn.btn-success').onclick = function () {
            addItemToBlock(block);
            updateNumbering();
            return false;
        };

        // Кнопка удалить элемент.
        block.querySelector('button.btn.btn-danger').onclick = function () {
            block.remove();
            updateNumbering();
            return false;
        };

        // Кнопка переместить блок вверх.
        block.querySelector('button.btn.btn-up').onclick = function () {
            moveItem(contentBlocks, block, 1);
            updateNumbering();
            return false;
        };

        // Кнопка переместить блок вниз.
        block.querySelector('button.btn.btn-down').onclick = function () {
            moveItem(contentBlocks, block, -1);
            updateNumbering();
            return false;
        };

        let contentItems = block.querySelector('.content-sub-item')
        // Пройти по элементам внутри блока.
        block.querySelectorAll('.sub-item').forEach((item) => {
            // Кнопка удалить элемент.
            item.querySelector('button.btn.btn-secondary').onclick = function () {
                item.remove();
                if (block.querySelectorAll('.mb-2').length == 0) {
                    block.remove();
                }
                updateNumbering();
                return false;
            };

            // Кнопка переместить элемент вверх.
            item.querySelector('button.btn.btn-up').onclick = function () {
                moveItem(contentItems, item, 1);
                updateNumbering();
                return false;
            };

            // Кнопка переместить элемент вниз.
            item.querySelector('button.btn.btn-down').onclick = function () {
                moveItem(contentItems, item, -1);
                updateNumbering();
                return false;
            };
        });
    });
}

/**
 * Обновить подписи и стили элементов в блоке.
 * @param {NodeListOf<Element>} items Элементы в блоке.
 * @param {number} selectedOption Выбранное значение в выпадающем списке.
 */
function updateSubItemFields(items, selectedOption) {
    items.forEach((item) => {
        changeStyle(item.children, selectedOption);
    });
}

/**
 * Обновить подпись и стиль элемента в блоке.
 * @param {HTMLCollection} item Элемент блока.
 * @param {number} selectedOption Выбранное значение в выпадающем списке.
 */
function changeStyle(item, selectedOption) {

    selectArray[selectedOption].items.forEach((select, index) => {
        item[index].style.display = select.display;
        item[index].children[0].textContent = select.title;
    })
}

/**
 * Добавить элемент к блоку.
 * @param {HTMLDivElement} block Блок к которому добавляется новый элемент.
 */
function addItemToBlock(block) {
    let blockItems = block.querySelector('.content-sub-item');

    // Контейнер для элемента.
    var item = document.createElement("div");
    item.className = "sub-item";
    item.dataset.itemNumber = block.dataset.blockNumber + ".1";

    // Значение из выпадающего списка.
    var number = block.querySelectorAll(".item-select")[0].value;

    // Добавить поля ввода и стилизовать в соответствии с выбранным значением.
    selectArray[number].items.forEach((select) => {
        // Контейнер для поля ввода
        var titleItem = document.createElement("div");
        titleItem.className = "mb-2";
        titleItem.style.display = select.display;

        // Подпись.
        var labelItem = document.createElement("label");
        labelItem.className = "form-label";
        labelItem.textContent = select.title;

        // Поле ввода.
        var InputItem = document.createElement("input");
        InputItem.type = "text";
        InputItem.className = "text-input edit";
        InputItem.id = select.name;
        InputItem.name = `Contents[${block.dataset.blockNumber}].Items[0].${select.name}`;

        titleItem.appendChild(labelItem);
        titleItem.appendChild(InputItem);

        // Контейнер для текстового редактора.
        if (select.name == "Text") {
            InputItem.style = "display: none;";
            let editor = document.createElement("div");
            editor.className = "editor";
            editor.id = `Contents${block.dataset.blockNumber}Items0Editor`;

            titleItem.appendChild(editor);
        }
        
        // Добавление поля для загрузки изображения.
        if (select.name == "ImageName") {
            InputItem.style = "display: none;";

            // Поле ввода для названия временного файла.
            var tempInputItem = document.createElement("input");
            tempInputItem.type = "text";
            tempInputItem.className = "text-input edit";
            tempInputItem.id = "TempImageName";
            tempInputItem.name = `Contents[${block.dataset.blockNumber}].Items[0].TempImageName`;
            tempInputItem.style = "display: none;";

            // Поле ввода для выбора изображения.
            var fileItem = document.createElement("input");
            fileItem.type = "file";
            fileItem.className = "text-input edit";
            fileItem.id = "File";
            fileItem.name = `Contents[${block.dataset.blockNumber}].Items[0].File`;
            fileItem.onchange = function () {
                updateItemPreview(this, `Contents[${block.dataset.blockNumber}].Items[0].Image`)
            };

            // Изображение.
            var imageItem = document.createElement("img");
            imageItem.className = "wrapper exmp edit";
            imageItem.id = `Contents[${block.dataset.blockNumber}].Items[0].Image`;
            imageItem.src = "https://placehold.co/600x350";

            titleItem.insertBefore(imageItem, titleItem.firstChild)
            titleItem.appendChild(tempInputItem);
            titleItem.appendChild(fileItem);
        }

        item.appendChild(titleItem);
    })

    // Кнопка удалить элемент.
    var removeItem = document.createElement("button");
    removeItem.textContent = "Удалить элемент";
    removeItem.type = "button";
    removeItem.className = "btn btn-secondary";
    removeItem.onclick = function () {
        item.remove();
        if (block.querySelectorAll('.mb-2').length == 0) {
            block.remove();
        }
        updateNumbering();
        return false;
    };

    // Кнопка переместить элемент вверх.
    let upItem = document.createElement("button");
    upItem.textContent = "↑";
    upItem.className = "btn btn-up";
    upItem.onclick = function () {
        moveItem(blockItems, item, 1);
        updateNumbering();
        return false;
    };

    // Кнопка переместить элемент вниз.
    let downItem = document.createElement("button");
    downItem.textContent = "↓";
    downItem.className = "btn btn-down";
    downItem.onclick = function () {
        moveItem(blockItems, item, -1);
        updateNumbering();
        return false;
    };

    item.appendChild(removeItem);
    item.appendChild(upItem);
    item.appendChild(downItem);

    blockItems.appendChild(item);
};

/**
 * Обновляет нумерацию элементов в конструкторе и
 * обновляет привязку редактора текста к текстовому
 * полю в соответствии с новой нумерацией.
 */
function updateNumbering() {
    const blocks = document.querySelectorAll('.item');

    // 
    blocks.forEach((block, blockIndex) => {
        block.dataset.blockNumber = blockIndex;

        // Выпадающий список.
        const blockSelect = block.querySelector('.item-select');
        blockSelect.name = `Contents[${blockIndex}].Type`;

        const items = block.querySelectorAll('.sub-item');
        items.forEach((item, itemIndex) => {

            item.dataset.itemNumber = blockIndex + '.' + itemIndex;

            // Обновление нумерации полей ввода.
            item.querySelectorAll('input').forEach((input) => {
                input.name = `Contents[${blockIndex}].Items[${(itemIndex)}].${input.id}`;
            });

            // Текстовый редактор.
            let editor = item.querySelector('.editor');
            editor.id = `Contents${blockIndex}Items${(itemIndex)}Editor`;

            // Превью изображения.
            let imageView = item.querySelector('.wrapper.exmp.edit');
            imageView.id = `Contents[${blockIndex}].Items[${itemIndex}].Image`;

            // Поле выбора файла.
            let fileOnChange = item.querySelector('input[type=file]');
            fileOnChange.onchange = function () {
                updateItemPreview(this, `Contents[${blockIndex}].Items[${itemIndex}].Image`)
            };

            const delay = 20;
            // Инициализация текстового редактора.
            setTimeout(() => {
                item.querySelectorAll(".ql-toolbar.ql-snow").forEach(e => e.remove());
                let editorId = `Contents${blockIndex}Items${(itemIndex)}Editor`;
                let inputId = `Contents[${blockIndex}].Items[${(itemIndex)}].Text`

                // Если есть содержимое в контейнере редактора, сохранить его во ременную переменную.
                let tempHtml;
                if (item.querySelector(".ql-editor") == null) {
                    tempHtml = "";
                }
                else {
                    tempHtml = item.getElementsByClassName("editor")[0].children[0].innerHTML;
                }

                // Инициализировать редактор.
                let editorObject = setEditor(`#${editorId}`);
                editorObject.on('text-change', (delta, oldDelta, source) => {
                    updateItemEditor(editorId, inputId);
                });

                // Вставить ранее сохраненное содержимое.
                if (tempHtml != "") {
                    // При инициализации, редактор по умолчанию добавляет шаблонный html код и перед вставкой, его нужно очистить.
                    editorObject.setText("");
                    editorObject.clipboard.dangerouslyPasteHTML(0, tempHtml);
                }

            }, delay);

        });
    });
}

/**
 * Добавить блок.
 */
function addBlock() {
    var block = document.createElement("div");
    block.className = "item";
    block.dataset.blockNumber = document.querySelectorAll('.item').length;

    // Выпадающий список.
    var selectOption = document.createElement("select");
    selectOption.className = "item-select";
    selectArray.forEach((item, index) => {
        selectOption.innerHTML += `<option value="${index}">${item.name}</option>`
    });
    selectOption.name = `Contents[${block.dataset.blockNumber}].Type`;
    selectOption.onchange = function () {
        updateSubItemFields(block.querySelectorAll('.sub-item'), selectOption.value);
    };

    // Кнопка добавить элемент.
    var addItemButton = document.createElement("button");
    addItemButton.textContent = "Добавить элемент";
    addItemButton.type = "button";
    addItemButton.className = "btn btn-success";
    addItemButton.onclick = function () {
        addItemToBlock(block);
        updateNumbering();
        return false;
    };

    // Кнопка удалить блок.
    var removeButton = document.createElement("button");
    removeButton.textContent = "Удалить блок";
    removeButton.type = "button";
    removeButton.className = "btn btn-danger";
    removeButton.onclick = function () {
        block.remove();
        updateNumbering();
        return false;
    };

    // Кнопка переместить блок вверх.
    let upButton = document.createElement("button");
    upButton.textContent = "↑";
    upButton.className = "btn btn-up";
    upButton.onclick = function () {
        moveItem(contentBlocks, block, 1);
        updateNumbering();
        return false;
    };

    // Кнопка переместить блок вниз.
    let downButton = document.createElement("button");
    downButton.textContent = "↓";
    downButton.className = "btn btn-down";
    downButton.onclick = function () {
        moveItem(contentBlocks, block, -1);
        updateNumbering();
        return false;
    };

    // Контейнер для элементов.
    let contentItems = document.createElement("div");
    contentItems.className = "content-sub-item";

    block.appendChild(selectOption);
    block.appendChild(addItemButton);
    block.appendChild(removeButton);
    block.appendChild(upButton);
    block.appendChild(downButton);
    block.appendChild(contentItems);
    addItemToBlock(block);

    contentBlocks.appendChild(block);

    updateNumbering();
}

/**
 * Перемещает блок/элемент вверх или вниз.
 * @param {HTMLElement} itemList Контейнер, содержащий в себе коллекцию элементов.
 * @param {HTMLDivElement} element Элемент который нужно переместить.
 * @param {number} direction На сколько нужно переместить элемент, 1 вверх, -1 вниз.
 */
function moveItem(itemList, element, direction) {
    const index = Array.from(itemList.children).indexOf(element);
    const newIndex = index - direction;

    if (newIndex >= 0 && newIndex < itemList.children.length) {
        // Меняет элементы местами
        const referenceNode = itemList.children[newIndex];
        itemList.insertBefore(element, direction === 1 ? referenceNode.nextSibling : referenceNode);
    }
}