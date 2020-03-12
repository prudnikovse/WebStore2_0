
Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: "",
        decrementLink: "",
        removeFromCartLink: "",
        removeAllLink: ""

    },
    init: function (properties) {
        $.extend(Cart._properties, properties)

        Cart.initEvents()
    },
    initEvents: function () {
        $(".add-to-cart").click(Cart.addToCart)
        $(".cart_quantity_up").click(Cart.incrementItem)
        $(".cart_quantity_down").click(Cart.decrementItem)
        $(".cart_quantity_delete").click(Cart.removeFromCart)
    },
    addToCart: function (event) {
        event.preventDefault()

        let button = $(this)
        const id = button.data("id")

        $.get(`${Cart._properties.addToCartLink}/${id}`)
            .done(() => {
                Cart.showToolTip(button)
                Cart.refreshCartView()
            })
            .fail(function () { console.log("Add to cart fail") })
    },
    showToolTip: function (button) {
        button.tooltip({ title: "добавлено в корзину" }).tooltip("show")
        setTimeout(function () { button.tooltip("destroy") }, 1000)
    },
    refreshCartView: function () {
        let container = $("#cart-container")

        $.get(Cart._properties.getCartViewLink)
            .done((content) => {
                container.html(content)
            })
            .fail(function () { console.log("refreshCartView fail") })
    },
    incrementItem: function (event) {
        event.preventDefault()

        let button = $(this)
        const id = button.data("id")

        let container = button.closest("tr")

        $.get(`${Cart._properties.addToCartLink}/${id}`)
            .done(() => {
                const count = parseInt($(".cart_quantity_input", container).val())
                $(".cart_quantity_input", container).val(count + 1)
                Cart.refreshPrice(container)
                Cart.refreshCartView()
            })
            .fail(function () { console.log("Add to cart fail") })
    },
    decrementItem: function (event) {
        event.preventDefault()

        let button = $(this)
        const id = button.data("id")

        let container = button.closest("tr")

        $.get(`${Cart._properties.decrementLink}/${id}`)
            .done(() => {
                const count = parseInt($(".cart_quantity_input", container).val())
                if (count > 1) {
                    $(".cart_quantity_input", container).val(count - 1)
                    Cart.refreshPrice(container)                  
                } else {
                    container.remove()
                    Cart.refreshTotalPrice()
                }

                Cart.refreshCartView()
            })
            .fail(() => console.log("Decrement from cart fail"))
    },
    removeFromCart: function (event) {
        event.preventDefault()

        let button = $(this)
        const id = button.data("id")

        $.get(`${Cart._properties.removeFromCartLink}/${id}`)
            .done(() => {
                button.closest("tr").remove();
                Cart.refreshTotalPrice()
                Cart.refreshCartView()
            })
            .fail(() => console.log("Remove from cart fail"))
    },
    refreshPrice: function (container) {
        const quantity = parseInt($(".cart_quantity_input", container).val())
        const price = parseFloat($(".cart_price", container).data("price"))
        const totalPrice = price * quantity

        const value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" })
        $(".cart_total_price", container).data("price", totalPrice)
        $(".cart_total_price", container).html(value)

        Cart.refreshTotalPrice();
    },
    refreshTotalPrice: function () {
        let total = 0;

        $(".cart_total_price").each(function () {
            const price = parseFloat($(this).data("price"))
            total += price
        })

        const value = total.toLocaleString("ru-RU", { style: "currency", currency: "RUB" })
        $("#total-order-sum").html(value)
    }
}