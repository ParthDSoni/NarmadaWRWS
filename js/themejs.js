

jQuery(document).ready(function () {
    /* change color */

    // function to set a given theme/color-scheme
    function setTheme(themeName) {
        localStorage.setItem('theme', themeName);
        document.documentElement.className = themeName;
    }

    // function to toggle between light and dark theme
    function toggleTheme() {
        if (localStorage.getItem('theme') === 'theme-dark') {
            setTheme('theme-light');
        } else {
            setTheme('theme-dark');
        }
    }

    // Immediately invoked function to set the theme on initial load
    (function () {
        if (localStorage.getItem('theme') === 'theme-dark') {
            setTheme('theme-dark');
            // document.getElementById('slider').checked = false;
        } else {
            setTheme('theme-light');
            // document.getElementById('slider').checked = true;
        }
    })();
});


jQuery(document).ready(function ($) {
    $("#dark-theme").click(function () {
/*        $("body").addClass("theme-dark");*/
        $("body").removeClass("theme-light").addClass("theme-dark");
    });
    $("#light-theme").click(function () {
/*        $("body").removeClass("theme-dark");*/
        $("body").removeClass("theme-dark").addClass("theme-light");
    });
});

$(function () {
	var $affectedElements = $("p, div, a, li, h2, h3, h4, h5, td");

	var flg = 1;
	var old_flg;

	// Store original font size
	$affectedElements.each(function () {
		var $this = $(this);
		$this.data("orig-size", parseFloat($this.css("font-size")));
	});

	// Increase font size
	$("#btn-increase").on("click", function (e) {
		e.preventDefault();
		old_flg = flg;
		flg++;
		changeFontSize(1.5);
	});


	// Decrease font size
	$("#btn-decrease").on("click", function (e) {
		e.preventDefault();
		old_flg = flg;
		flg--;
		changeFontSize(-1.5);
	});

	// Reset font size
	$("#btn-orig").on("click", function (e) {
		e.preventDefault();
		old_flg = flg;
		flg = 1;

		$affectedElements.each(function () {
			var $this = $(this);
			var originalSize = $this.data("orig-size");
			if (originalSize) {
				$this.css("font-size", originalSize + "px");
			}
		});
	});

	// Font size update function
	function changeFontSize(step) {
		if (flg >= 0 && flg <= 2) {
			$affectedElements.each(function () {
				var $this = $(this);
				var currentSize = parseFloat($this.css("font-size"));
				var newSize = currentSize + step;

				if (newSize >= 10 && newSize <= 50) {
					$this.css("font-size", newSize + "px");
				}
			});
		} else {
			flg = old_flg;
		}
	}
});
