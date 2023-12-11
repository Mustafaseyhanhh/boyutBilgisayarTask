var sepet = $.parseJSON(localStorage.localBasket) ?? {} ;

function addBasket(id, name, price, stock) {
	var adetID = "#"+id + "-adet";
	var adet = $(adetID).val();
	if (adet != "" && adet != "0") {
		addBasketJson(id, name, price, adet, stock);


	} else {
		Swal.fire({
			text: "Adet Girmelisiniz",
			icon: "error",
			buttonsStyling: false,
			confirmButtonText: "Tamam",
			customClass: {
				confirmButton: "btn btn-primary"
			}
		});
	}
	//durumYazdir();
}

function addBasketJson(id, name, price, adet, stock) {
	durumYazdir();
	var eski = sepetControl(id);
	if ((eski + parseFloat(adet)) > stock) {
		Swal.fire({
			text: "Toplam Adetten fazla sipariş veremezsiniz",
			icon: "error",
			buttonsStyling: false,
			confirmButtonText: "Tamam",
			customClass: {
				confirmButton: "btn btn-primary"
			}
		});
	} else {
		var totalPrice = ((parseFloat(adet) + eski).toFixed(2) * parseFloat(price.replace(",",".")).toFixed(2)).toFixed(2);
		$("#" + id + "-card").remove();
		var item = { id: id, name: name, price: price.replace(",", "."), adet: (parseFloat(adet) + eski), totalPrice: totalPrice }
		console.log(item);
		sepet[id] = item;
		localStorage.localBasket = JSON.stringify(sepet);
		var card = buildCard(id, name, price.replace(",", "."), parseFloat(adet) + eski, totalPrice);
		$("#bassket").append(card);
		toplamDegistir();
		Swal.fire({
			text: "Ürün Başarı ile sepetinize eklendi",
			icon: "success",
			buttonsStyling: false,
			confirmButtonText: "Tamam",
			customClass: {
				confirmButton: "btn btn-primary"
			}
		});
	}
}

function toplamDegistir() {
	var toplam=0;
	for (var key in sepet) {  //loop through the array
		toplam = parseFloat(toplam) + parseFloat(sepet[key].totalPrice);  //Do the math!
		console.log(toplam);
	}
	var ff = $("#toplamTutar");
	var toplamStr = toplam.toFixed(2).toString() + " ₺";
	console.log(toplamStr);
	ff.text(toplamStr);
	localStorage.toplamTutar = toplamStr;
}

function sepetControl(id) {
	if (sepet != null) {
		if (id in sepet) {
			return parseFloat(sepet[id].adet);
		} else {
			return 0;

		}
	} else {
		return 0;
	}
	
	
}

function buildCard(id, name, price, adet, totalPrice) {
	return `<div b-akl1e1kvgi="" class="d-flex flex-stack py-4 ms-10 me-10" id="${id}-card">
                        <!--begin::Section-->
                        <div b-akl1e1kvgi="" class="d-flex align-items-center">
                            <!--begin::Symbol-->
                            <div b-akl1e1kvgi="" class="symbol symbol-35px me-4">
                                <span b-akl1e1kvgi="" class="symbol-label bg-light-primary">      
                                    <i b-akl1e1kvgi="" class="ki-duotone ki-abstract-28 fs-2 text-primary"><span b-akl1e1kvgi="" class="path1"></span><span b-akl1e1kvgi="" class="path2"></span></i>                                                    
                                </span>
                            </div>
                            <!--end::Symbol-->

                            <!--begin::Title-->
                            <div b-akl1e1kvgi="" class="mb-0 me-2">
                                <a b-akl1e1kvgi="" href="#" class="fs-6 text-gray-800 text-hover-primary fw-bold">${name}</a>
                                <div b-akl1e1kvgi="" class="text-gray-500 fs-7">${price} ₺</div>
                            </div>
                            <!--end::Title-->
                        </div>
                        <!--end::Section-->

                        <!--begin::Label-->
                        <span b-akl1e1kvgi="" class="badge badge-light fs-8" id="${id}-adet">${adet} Adet</span>
                        <span b-akl1e1kvgi="" class="badge badge-light fs-8">${totalPrice} ₺</span>
                        <!--end::Label-->
                    </div>`
}

function durumYazdir() {
	console.log("localBasket");
	console.log(localStorage.localBasket);
	console.log("sepet");
	console.log(sepet);
}

function OdemeSayfasi() {
	window.location.href("/")
}