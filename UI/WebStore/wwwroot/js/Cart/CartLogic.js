
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

        fetch(`${Cart._properties.addToCartLink}/${id}`)
            .then(() => {
                Cart.showToolTip(button)
                Cart.refreshCartView()
            })
            .catch(res => { console.log(`Add to cart fail. ${res.statusText}`) })
    },
    showToolTip: function (button) {
        button.tooltip({ title: "добавлено в корзину" }).tooltip("show")
        setTimeout(() => button.tooltip("destroy"), 1000)
    },
    refreshCartView: async function() {
        let container = $("#cart-container")

        var response = await fetch(Cart._properties.getCartViewLink)

        if (response.ok) 
            container.html(await response.text())
        else
            console.log(`refreshCartView fail. ${response.statusText}`)
    },
    incrementItem: function (event) {
        event.preventDefault()

        let button = $(this)
        const id = button.data("id")

        let container = button.closest("tr")

        fetch(`${Cart._properties.addToCartLink}/${id}`)
            .then(() => {
                const count = parseInt($(".cart_quantity_input", container).val())
                $(".cart_quantity_input", container).val(count + 1)
                Cart.refreshPrice(container)
                Cart.refreshCartView()
            })
            .catch(res => { console.log(`Add to cart fail. ${res.statusText}`) })
    },
    decrementItem: function (event) {
        event.preventDefault()

        let button = $(this)
        const id = button.data("id")

        let container = button.closest("tr")

        fetch(`${Cart._properties.decrementLink}/${id}`)
            .then(() => {
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
            .catch(res => { console.log(`Decrement from cart fail. ${res.statusText}`) })      
    },
    removeFromCart: function (event) {
        event.preventDefault()

        let button = $(this)
        const id = button.data("id")

        fetch(`${Cart._properties.removeFromCartLink}/${id}`)
            .then(() => {
                button.closest("tr").remove();
                Cart.refreshTotalPrice()
                Cart.refreshCartView()
            })
            .catch(res => { console.log(`Remove from cart fail. ${res.statusText}`) })
    },
    refreshPrice: function(container) {
        const quantity = parseInt($(".cart_quantity_input", container).val())
        const price = parseFloat($(".cart_price", container).data("price"))
        const totalPrice = price * quantity

        const value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" })
        $(".cart_total_price", container).data("price", totalPrice)
        $(".cart_total_price", container).html(value)

        Cart.refreshTotalPrice();
    },
    refreshTotalPrice: function() {
        let total = 0;

        $(".cart_total_price").each(function () {
            const price = parseFloat($(this).data("price"))
            total += price
        })

        const value = total.toLocaleString("ru-RU", { style: "currency", currency: "RUB" })
        $("#total-order-sum").html(value)
    }
}