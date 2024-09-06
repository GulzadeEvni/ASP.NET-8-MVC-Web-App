// Sepet sayfası için
$(document).ready(function () {
    $('#completeShoppingButton').click(function () {
        // Toplam fiyatı al
        const totalAmount = parseFloat($('#totalPrice').text().replace(' TL', '').replace(',', '.')); // TL birimini kaldır ve virgülü ondalık olarak ayarla

        $.ajax({
            url: '/Cart/CompleteShopping',
            type: 'POST',
            data: { totalAmount: totalAmount }, // Toplam fiyatı gönder
            success: function (response) {
                if (response.redirect) {
                    window.location.href = response.redirectUrl;
                } else {
                    alert(response.message);
                }
            },
            error: function (error) {
                console.error("Alışveriş tamamlamada hata: ", error);
                alert('Bir hata oluştu. Lütfen tekrar deneyin.');
            }
        });
    });

    // Sayfa yüklendiğinde toplam fiyatı güncelle
    updateTotalPrice();
});


function updateTotalPrice() {
    let totalPrice = 0;
    $('.flex[data-product-id]').each(function () {   //seçilen her bir öğe üzerinde döngü yaparak işlemleri teker teker gerçekleştir.sepetteki her bir ürün üzerinde işlem yap.
        const price = parseFloat($(this).data('product-price')); // $(this) ifadesi, mevcut döngüdeki HTML öğesini temsil eder.($ -> jOuery de seçici)
        const quantity = parseInt($(this).find('[id^="quantity-"]').text());// adeti, text ögesinden alıp int'e çevir 
        totalPrice += price * quantity;
    });
    $('#totalPrice').text(totalPrice.toFixed(2) + ' TL'); // 2 ondalıklı formata çevir ve text'e yaz
}

function changeQuantity(productId, change) {
    $.ajax({
        url: '/Cart/UpdateQuantity',
        type: 'POST',
        data: { productId: productId, change: change },
        success: function (response) {
            if (response.success) {
                $('#quantity-' + productId).text(response.newQuantity);
                updateTotalPrice(); // Toplam fiyatı güncelle

                if (response.newQuantity === 0) {
                    $('div[data-product-id="' + productId + '"]').remove();
                }

                updateCartIconCount(response.cartCount); // Sepet ikonundaki sayıyı güncelle
            } else {
                alert('Sepet miktarı güncellenirken bir hata oluştu.');
            }
        },
        error: function (error) {
            console.error("Miktar güncelleme hatası: ", error);
            alert('Bir hata oluştu. Lütfen tekrar deneyin.');
        }
    });
}

function removeFromCart(productId) {
    $.ajax({
        url: '/Cart/RemoveFromCart',
        type: 'POST',
        data: { productId: productId },
        success: function (response) {
            if (response.success) {
                // Ürünü sepetten kaldır
                $('div[data-product-id="' + productId + '"]').remove();

                // Sepet ikonundaki toplam ürün miktarını güncelle
                updateCartIconCount(response.cartCount);

                alert('Ürün sepetten kaldırıldı!');
            } else {
                alert('Ürünü sepetten kaldırırken bir hata oluştu.');
            }
        },
        error: function (error) {
            console.error("Ürünü kaldırma hatası: ", error);
            alert('Bir hata oluştu. Lütfen tekrar deneyin.');
        }
    });
}


// Index Sayfası için
function addToCart(productId) {
    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: { productId: productId },
        success: function (response) {
            if (response.success) {
                updateCartIconCount(response.cartCount);
                alert('Ürün sepete eklendi!');
            } else {
                alert('Sepete eklenirken bir hata oluştu.');
            }
        },
        error: function (error) {
            console.error("Sepete ekleme hatası: ", error);
            alert('Bir hata oluştu. Lütfen tekrar deneyin.');
        }
    });
}


function updateCartIconCount(cartCount) {
    $('#cartIcon').text(cartCount);
}




