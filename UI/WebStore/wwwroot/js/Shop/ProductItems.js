
ProductItems = {
    _properties: {
        getUrl: '',
    },
    init: prop => {
        $.extend(ProductItems._properties, prop)
        $('.pagination li a').click(ProductItems.clickOnPage)
    },
    clickOnPage: function (event) {
        event.preventDefault()

        const button = $(this)

        if (button.prop('href').length > 0) {
            let page = button.data('page')
            const container = $('#catalog-items-container')

            container.LoadingOverlay('show')
            const data = button.data()

            let query = ''
            for (let key in data)
                if (data.hasOwnProperty(key))
                    query += `${key}=${data[key]}&`

            fetch(`${ProductItems._properties.getUrl}?${query}`)
                .then(res => {
                    return res.text()
                })
                .catch(() => {
                    console.log('request get product fail')
                })
                .then(html => {
                    container.html(html)
                    container.LoadingOverlay('hide')

                    $('.pagination li').removeClass('active')
                    $('.pagination li a').prop('href', '#')
                    $(`.pagination li a[data-page=${page}]`)
                        .removeAttr('href')
                        .parent()
                        .addClass('active')

                    Cart.initEvents()
                })
                .catch(() => {
                    container.LoadingOverlay('hide')
                    console.log('get items fail')
                })
        } 
    }
}