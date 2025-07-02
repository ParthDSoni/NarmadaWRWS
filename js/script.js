
(function ($) {

	"use strict";


	//Update Header Style and Scroll to Top
	function headerStyle() {
		if ($('.main-header').length) {
			var windowpos = $(window).scrollTop();
			var siteHeader = $('.main-header');
			var scrollLink = $('.scroll-top');
			if (windowpos >= 110) {
				siteHeader.addClass('fixed-header');
				scrollLink.addClass('open');
			} else {
				siteHeader.removeClass('fixed-header');
				scrollLink.removeClass('open');
			}
		}
	}

	headerStyle();

	/* ==========================================================================
	 Submenu Dropdown Toggle
	 ========================================================================== */

	if ($('.main-header li.dropdown ul').length) {
		$('.main-header .navigation li.dropdown').append('<div class="dropdown-btn"><span class="fas fa-angle-down"></span></div>');

	}

	//Mobile Nav Hide Show
	if ($('.mobile-menu').length) {

		$('.mobile-menu .menu-box').mCustomScrollbar();

		var mobileMenuContent = $('.main-header .menu-area .main-menu').html();
		$('.mobile-menu .menu-box .menu-outer').append(mobileMenuContent);
		$('.sticky-header .main-menu').append(mobileMenuContent);

		//Dropdown Button
		$('.mobile-menu li.dropdown .dropdown-btn').on('click', function () {
			$(this).toggleClass('open');
			$(this).prev('ul').slideToggle(500);
		});
		//Dropdown Button
		$('.mobile-menu li.dropdown .dropdown-btn').on('click', function () {
			$(this).prev('.megamenu').slideToggle(900);
		});
		//Menu Toggle Btn
		$('.mobile-nav-toggler').on('click', function () {
			$('body').addClass('mobile-menu-visible');
		});

		//Menu Toggle Btn
		$('.mobile-menu .menu-backdrop,.mobile-menu .close-btn').on('click', function () {
			$('body').removeClass('mobile-menu-visible');
		});
	}


	/* ==========================================================================
	 Scroll to a Specific Div
	 ========================================================================== */
	if ($('.scroll-to-target').length) {
		$(".scroll-to-target").on('click', function () {
			var target = $(this).attr('data-target');
			// animate
			$('html, body').animate({
				scrollTop: $(target).offset().top
			}, 1000);

		});
	}

	/* ==========================================================================
	 LightBox / Fancybox
	 ========================================================================== */

	if ($('.lightbox-image').length) {
		$('.lightbox-image').fancybox({
			openEffect: 'fade',
			closeEffect: 'fade',
			helpers: {
				media: {}
			}
		});
	}


	/* ==========================================================================
	 Testimonial Carousel Four using award function index page
	 ========================================================================== */

	if ($('.testimonial-carousel-4').length) {
		$('.testimonial-carousel-4').owlCarousel({
			loop: true,
			margin: 0,
			nav: false,
			smartSpeed: 2000,
			autoplay: true,
			mouseDrag: false,
			touchDrag: true,
			navText: ['<span class="fas fa-angle-right"></span>', '<span class="fas fa-angle-left"></span>'],
			responsive: {
				0: {
					items: 1
				},
				480: {
					items: 1
				},
				600: {
					items: 1
				},
				800: {
					items: 1
				},
				1200: {
					items: 1
				}

			}
		});
	}


	/* ==========================================================================
	 //two-column-carousel policy index page
	 ========================================================================== */

	if ($('.two-column-carousel-2').length) {
		$('.two-column-carousel-2').owlCarousel({
			loop: true,
			margin: 30,
			nav: true,
			smartSpeed: 3000,
			draggable: false,
			mouseDrag: false,
			touchDrag: true,
			autoplay: 4000,
			navText: ['<span class="fas fa-arrow-left"></span>', '<span class="fas fa-arrow-right"></span>'],
			responsive: {
				0: {
					items: 1
				},
				480: {
					items: 1
				},
				600: {
					items: 1
				},
				800: {
					items: 2
				},
				1024: {
					items: 2
				}
			}
		});
	}
	/* ==========================================================================
	 // window load
	 ========================================================================== */
	$(window).on('load', function () {
		$("#loading").delay(300).fadeOut(300);
		$("#loading-center").on('click', function () {
			$("#loading").fadeOut(300);
		})
	})

	// Logo Slider JS
	$('.logo-slider').owlCarousel({
		loop: true,
		margin: 30,
		nav: false,
		dots: false,
		smartSpeed: 500,
		autoplay: true,
		autoplayTimeout: 3000,
		autoplayHoverPause: true,
		responsive: {
			0: {
				items: 2,
			},
			600: {
				items: 3,
			},
			1000: {
				items: 4,
			}
		}
	});


	/* ==========================================================================
	//three-item-carousel project index page
	========================================================================== */

	if ($('.three-item-carousel').length) {
		$('.three-item-carousel').owlCarousel({
			loop: true,
			margin: 30,
			nav: true,
			smartSpeed: 3000,
			dots: false,
			mouseDrag: true,
			touchDrag: true,
			autoplay: false,
			navText: ['<span class="flaticon-left"></span>', '<span class="fas fa-chevron-right"></span>'],
			responsive: {
				0: {
					items: 1
				},
				480: {
					items: 1
				},
				600: {
					items: 2
				},
				800: {
					items: 2
				},
				1024: {
					items: 3
				}
			}
		});
	}


		/* ==========================================================================
	//three-item-carousel project index page
	========================================================================== */

	if ($('.media-carousel').length) {
		$('.media-carousel').owlCarousel({
			loop: true,
			margin: 30,
			nav: true,
			smartSpeed: 3000,
			dots: false,
			mouseDrag: true,
			touchDrag: true,
			autoplay: false,
			navText: ['<span class="flaticon-left"></span>', '<span class="fas fa-chevron-right"></span>'],
			responsive: {
				0: {
					items: 1
				},
				480: {
					items: 1
				},
				600: {
					items: 2
				},
				800: {
					items: 2
				},
				1024: {
					items: 3
				}
			}
		});
	}



	    // single-item-carousel
		//if ($('.single-item-carousel').length) {
		//	$('.single-item-carousel').owlCarousel({
		//		loop:true,
		//		margin:30,
		//		nav:true,
		//		smartSpeed: 500,
		//		autoplay: false,
		//		navText: [ '<span class="flaticon-left-arrow"></span>', '<span class="flaticon-right-arrow"></span>' ],
		//		responsive:{
		//			0:{
		//				items:1
		//			},
		//			480:{
		//				items:1
		//			},
		//			600:{
		//				items:1
		//			},
		//			800:{
		//				items:1
		//			},			
		//			1200:{
		//				items:1
		//			}
	
		//		}
		//	});    		
		//}

	//Add One Page nav
	if ($('.scroll-nav').length) {
		$('.scroll-nav').onePageNav();
	}
	//Search Popup
	if ($('#search-popup').length) {

		//Show Popup
		$('.search-toggler').on('click', function () {
			$('#search-popup').addClass('popup-visible');
		});
		$(document).keydown(function (e) {
			if (e.keyCode === 27) {
				$('#search-popup').removeClass('popup-visible');
			}
		});
		//Hide Popup
		$('.close-search,.search-popup .overlay-layer').on('click', function () {
			$('#search-popup').removeClass('popup-visible');
		});
	}

	/* ==========================================================================
		//Main Slider Carousel using indexpage//
	 ========================================================================== */

	if ($('.main-slider-carousel').length) {
		$('.main-slider-carousel').owlCarousel({
			loop: true,
			margin: 0,
			nav: true,
			animateOut: 'fadeOut',
			animateIn: 'fadeIn',
			dots: false,
			active: true,
			smartSpeed: 1000,
			draggable: false,
			swipeToSlide: false,
			mouseDrag: false,
			touchDrag: true,
			autoplay: 6000,
			navText: ['<span class="fas fa-angle-right"></span>', '<span class="fas fa-angle-left"></span>'],
			responsive: {
				0: {
					items: 1
				},
				600: {
					items: 1
				},
				1200: {
					items: 1
				}
			}
		});
	}




	/* ==========================================================================
	 Current circular index page
	 ========================================================================== */

	// $(function () {
	// 	var tickerLength = $('.carousel-inner-data ul li').length;
	// 	var tickerHeight = $('.carousel-inner-data ul li').outerHeight();
	// 	$('.carousel-inner-data ul li:last-child').prependTo('.carousel-inner-data ul');
	// 	$('.carousel-inner-data ul').css('marginTop', -tickerHeight);

	// 	function moveTop() {
	// 		$('.carousel-inner-data ul').animate({
	// 			top: -tickerHeight
	// 		}, 600, function () {
	// 			$('.carousel-inner-data ul li:first-child').appendTo('.carousel-inner-data ul');
	// 			$('.carousel-inner-data ul').css('top', '');
	// 		});

	// 	}
	// 	setInterval(function () {
	// 		moveTop();
	// 	}, 3000);
	// });

	/* ==========================================================================
	 When document is Scrollig, do
	 ========================================================================== */

	$(window).on('scroll', function () {
		headerStyle();
	});
	/* ==========================================================================
				 font change js
				 ========================================================================== */
	var $affectedElements = $("p,div,a,li,h2,h3,h4,h5,td"); // Can be extended, ex. $("div, p, span.someClass")
	var flg = 1;
	var old_flg;

	// Storing the original size in a data attribute so size can be reset
	$affectedElements.each(function () {
		var $this = $(this);
		$this.data("orig-size", $this.css("font-size"));
	});

	$("#btn-increase").click(function () {
		old_flg = flg;
		flg++;
		changeFontSize(1.5);
	})

	$("#btn-decrease").click(function () {
		old_flg = flg;
		flg--;
		changeFontSize(-1.5);
	})

	$("#btn-orig").click(function () {
		old_flg = flg;
		flg = 1;
		$affectedElements.each(function () {
			var $this = $(this);
			$this.css("font-size", $this.data("orig-size"));
		});
	})

	function changeFontSize(direction) {
		if (flg >= 0 && flg <= 2) {
			$affectedElements.each(function () {
				var $this = $(this);
				$this.css("font-size", parseInt($this.css("font-size")) + direction);
			});
		}
		else {
			flg = old_flg;
		}
	}
	/* ==========================================================================
	 When document is loaded, do
	 ========================================================================== */


})(window.jQuery);


