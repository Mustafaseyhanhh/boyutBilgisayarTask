var sepet = $.parseJSON(localStorage.localBasket);

var ff = $("#toplamTutar");
var toplamStr = localStorage.toplamTutar;

for (var key in sepet) {  //loop through the array
	var item = buildItem(sepet[key].name, sepet[key].price, sepet[key].adet, sepet[key].totalPrice);
	$("#eklenecek").append(item);
}

ff.text(toplamStr);


function buildItem(name, price, adet, totalPrice) {
	return `<tr class="fw-bolder text-gray-700 fs-5 text-end">
				<td class="d-flex align-items-center pt-6">
				<i class="fa fa-genderless text-danger fs-2 me-2"></i>${name}</td>
				<td class="pt-6">${price} ₺</td>
				<td class="pt-6">${adet}</td>
				<td class="pt-6 text-dark fw-boldest">${totalPrice} ₺</td>
			</tr>`
}


function basketComplated() {
	var sepetArr = [];
	for (var key in sepet) {  //loop through the array
		sepetArr.push(sepet[key]);
	}


	var request = $.ajax({
		url: "/order/BasketComplated",
		type: "POST",
		data: { json: JSON.stringify(sepetArr) },
		dataType: "text"
	});

	request.done(function (msg) {
		console.log(msg);
		localStorage.localBasket = null;
		localStorage.toplamTutar = "0  ₺"
		window.location.replace('/order');
	});

	request.fail(function (jqXHR, textStatus) {
		alert("Request failed: " + textStatus);
	});
}