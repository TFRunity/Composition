var Modal = document.getElementById('exModal')
if (Modal) {
    Modal.addEventListener('show.bs.modal', function (event) {
        var button = event.relatedTarget
        //////////////////////////////////////////////////////Получаю свойства
        //Получаю постоянные свойства (не изменяющиеся)
        var productid = button.getAttribute('data-bs-productid')
        var email = button.getAttribute('data-bs-email')
        var name = button.getAttribute('data-bs-name')
        //Можно и через Ajax, но сроки поджимают
        //Получаю картинки, цены, айди подтипа и описания

        //Ссылки на картинки
        var firstpic = button.getAttribute('data-bs-firsturl')
        var secondpic = button.getAttribute('data-bs-secondurl')
        var thirdpic = button.getAttribute('data-bs-thirdurl')

        //Айди
        var firstsubcategoryid = button.getAttribute('data-bs-firstsubcategoryid')
        var secondsubcategoryid = button.getAttribute('data-bs-secondsubcategoryid')
        var thirdsubcategoryid = button.getAttribute('data-bs-thirdsubcategoryid')

        //Описание
        var firstfeature = button.getAttribute('data-bs-firstfeature')
        var secondfeature = button.getAttribute('data-bs-secondfeature')
        var thirdfeature = button.getAttribute('data-bs-thirdfeature')

        //Цены
        var firstprice = button.getAttribute('data-bs-firstprice')
        var secondprice = button.getAttribute('data-bs-secondprice')
        var thirdprice = button.getAttribute('data-bs-thirdprice')
        ////////////////////////////////////////////////////////////Нахожу класс в данном объекте

        //Постоянные данные
        var modalname = document.getElementById('ModalTitle')
        var modalemail = document.getElementById('email')
        var modalproductid = document.getElementById('productid')

        //Второстепенные данные

        //Картинки
        var modalfirstpic = document.getElementById('firstpic')
        var modalsecondpic = document.getElementById('secondpic')
        var modalthirdpic = document.getElementById('thirdpic')

        //Цены
        var modalfirstprice = document.getElementById('firstprice')
        var modalsecondprice = document.getElementById('secondprice')
        var modalthirdprice = document.getElementById('thirdprice')

        //Айди
        var modalfirstsubcategoryid = document.getElementById('firstsubcategoryid')
        var modalsecondsubcategoryid = document.getElementById('secondsubcategoryid')
        var modalthirdsubcategoryid = document.getElementById('thirdsubcategoryid')

        //Описание
        var modalfirstfeature = document.getElementById('firstfeature')
        var modalsecondfeature = document.getElementById('secondfeature')
        var modalthirdfeature = document.getElementById('thirdfeature')

        //Изменяю данные найденных классов новыми (полученными) свойствами

        //Постоянные данные
        modalname.textContent = name
        modalemail.value = email
        modalproductid.value = productid

        //Второстепенные данные

        //Картинки
        modalfirstpic.src = firstpic
        modalsecondpic.src = secondpic
        modalthirdpic.src = thirdpic

        //Цены
        modalfirstprice.textContent = firstprice + "₽"
        modalsecondprice.textContent = secondprice + "₽"
        modalthirdprice.textContent = thirdprice + "₽"

        //Айди
        modalfirstsubcategoryid.value = firstsubcategoryid
        modalsecondsubcategoryid.value = secondsubcategoryid
        modalthirdsubcategoryid.value = thirdsubcategoryid

        //Описания
        modalfirstfeature.textContent = firstfeature
        modalsecondfeature.textContent = secondfeature
        modalthirdfeature.textContent = thirdfeature
    })
}
