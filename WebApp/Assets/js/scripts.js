(function ($) {

    /*-----------------------------------------
        Responsive Menu
 -------------------------------------------*/

    window.selectnav = function () { "use strict"; var a = function (a, b) { function l(a) { var b; a || (a = window.event), a.target ? b = a.target : a.srcElement && (b = a.srcElement), b.nodeType === 3 && (b = b.parentNode), b.value && (window.location.href = b.value) } function m(a) { var b = a.nodeName.toLowerCase(); return b === "ul" || b === "ol" } function n(a) { for (var b = 1; document.getElementById("selectnav" + b); b++); return a ? "selectnav" + b : "selectnav" + (b - 1) } function o(a) { i++; var b = a.children.length, c = "", k = "", l = i - 1; if (!b) return; if (l) { while (l--) k += g; k += " " } for (var p = 0; p < b; p++) { var q = a.children[p].children[0], r = q.innerText || q.textContent, s = ""; d && (s = q.className.search(d) !== -1 || q.parentElement.className.search(d) !== -1 ? j : ""), e && !s && (s = q.href === document.URL ? j : ""), c += '<option value="' + q.href + '" ' + s + ">" + k + r + "</option>"; if (f) { var t = a.children[p].children[1]; t && m(t) && (c += o(t)) } } return i === 1 && h && (c = '<option value="">' + h + "</option>" + c), i === 1 && (c = '<select class="selectnav" id="' + n(!0) + '">' + c + "</select>"), i-- , c } a = document.getElementById(a); if (!a) return; if (!m(a)) return; document.documentElement.className += " js"; var c = b || {}, d = c.activeclass || "active", e = typeof c.autoselect == "boolean" ? c.autoselect : !0, f = typeof c.nested == "boolean" ? c.nested : !0, g = c.indent || "→", h = c.label || "- Navigation -", i = 0, j = " selected "; a.insertAdjacentHTML("afterend", o(a)); var k = document.getElementById(n()); return k.addEventListener && k.addEventListener("change", l), k.attachEvent && k.attachEvent("onchange", l), k }; return function (b, c) { a(b, c) } }();

    $('#menu section').children('ul').attr('id', 'tiny');

    selectnav('tiny', {
        label: '--- MENÜ --- ',
        indent: '-'
    });


	/*-----------------------------------------
	    	Initialize Flex Slider Carousels
	 -------------------------------------------*/

    $('html').removeClass('no-js');

    $('.rotator').each(function () {

        if ($(this).parent().hasClass('news') || $(this).parent().hasClass('tour')) {

            $(this).flexslider({
                animation: 'slide',
                animationLoop: false,
                slideshow: false,
                smoothHeight: true,
                controlsContainer: $(this).parent().find('.arrows'),
                keyboard: false,
                controlNav: false,
                video: true,

            });

        }
        else if ($(this).parent().hasClass('haberlerana')) {

            $(this).flexslider({
                animation: 'slide',
                animationLoop: false,
                slideshow: false,
                smoothHeight: true,
                controlsContainer: $(this).parent().find('.arrows'),
                keyboard: false,
                controlNav: false,
                video: true,
                itemHeight: 150

            });

        }

        else if ($(this).parent().hasClass('event')) {

            $(this).flexslider({
                animation: 'slide',
                animationLoop: false,
                slideshow: false,
                smoothHeight: true,
                controlsContainer: $(this).parent().find('.arrows'),
                keyboard: false,
                controlNav: false,
                itemWidth: 163
            });

        }

    });

    /*-----------------------------------------
        Initialize Homepage Slider
 -------------------------------------------*/

    if ($('#homeSlider').length > 0) {

        var hIndex = 0;
        var $hCap = $('#homeSlider').find('.captions').children('li'), $hSlides, $hPrev, $hNext;

        $('#homeSlider').flexslider({
            animation: 'slide',
            animationLoop: true,
            slideshow: true,
            slideshowSpeed: 7000,
            smoothHeight: false,

            controlsContainer: $('#homeSlider').find('.thumbs'),
            keyboard: true,
            controlNav: false,
            start: function (e) {

                $hPrev = $('#homeSlider').find('.flex-prev');
                $hNext = $('#homeSlider').find('.flex-next');
                $hSlides = $('#homeSlider').find('.slides').children('li');

                $hPrev.append('<img src="' + $hSlides.eq(e.count).data('thumb') + '" />');
                $hNext.append('<img src="' + $hSlides.eq(hIndex + 2).data('thumb') + '" />');

                $hPrev = $hPrev.find('img');
                $hNext = $hNext.find('img');

            },
            before: function (e) {

                $hCap.eq(hIndex).fadeOut(200);

                var prev = hIndex - 1 < 0 ? e.count - 1 : hIndex - 1;
                var next = hIndex + 1 > e.count - 1 ? 0 : hIndex + 1;

                hIndex = e.data('flexslider').animatingTo;
                $hCap.eq(hIndex).fadeIn(500);

                changeThumbs(prev, next);

            }
        });

        function changeThumbs(prev, next) {
            $hPrev.prop('src', $hSlides.eq(prev).data('thumb'));
            $hNext.prop('src', $hSlides.eq(next + 2).data('thumb'));
        }

    }

    /*-----------------------------------------
        Input Replacement
 -------------------------------------------*/

    $('input, textarea').each(function () {

        if (!$(this).hasClass('submit') && $(this).attr('id') != 'submit' && $(this).attr('id') != 'contactSubmit') {
            $(this).attr('data-value', $(this).val())
                .focus(function () {
                    $(this).addClass('focusInput');
                    if ($(this).val() == $(this).attr('data-value')) {
                        $(this).val('');
                    } else {
                        $(this).select();
                    }
                })
                .blur(function () {
                    $(this).removeClass('focusInput');
                    if ($(this).val() == '') {
                        $(this).val($(this).attr('data-value'));
                    }
                });
        }

    });

    /*-----------------------------------------
	    	Grayscale pictures effect
	 -------------------------------------------*/

    $('.grayColor').each(function () {

        var src = $(this).children('img').data('color');

        if (src != undefined) {
            $(this).append('<img class="overlay" src="' + src + '" alt="" />');
        }

    });

	/*-----------------------------------------
	    	Submenus
	 -------------------------------------------*/

    $('#menu').find('ul.clearfix').children('li').hover(function () {

        $(this).find('ul.sub-menu').stop().slideDown(150);
    }, function () {


        $(this).find('ul.sub-menu').stop().slideUp(100);
    });


	/*-----------------------------------------
	    	Contact Form Handler
	 -------------------------------------------*/

    if ($('#contact').length > 0) {

        var $name = $('#contactName');
        var $email = $('#contactMail');
        var $message = $('#contactMessage');
        var $error = $('#contactError');
        var $success = $('#contactSuccess');

        $('#contactSubmit').click(function () {

            var ok = true;
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;

            if ($name.val().length < 3 || $name.val() == $name.data('value')) {
                showError($name);
                ok = false;
            }

            if ($email.val() == '' || $email.val() == $email.data('value') || !emailReg.test($email.val())) {
                showError($email);
                ok = false;
            }

            if ($message.val().length < 5 || $message.val() == $message.data('value')) {
                showError($message);
                ok = false;
            }

            function showError($input) {
                $input.val($input.data('value'));
                $input.addClass('contactErrorBorder');
                $error.fadeIn();
            }

            if (ok) {

                $('#contact').fadeOut();

                $.ajax({
                    type: 'POST',
                    url: 'contact-form.php',
                    data: 'name=' + $name.val() + '&email=' + $email.val() + '&message=' + $message.val(),
                    success: function () {
                        $success.fadeIn();
                    }
                });

            }

            return false;

        });

        $name.focus(function () { resetError($(this)) });
        $email.focus(function () { resetError($(this)) });
        $message.focus(function () { resetError($(this)) });

        function resetError($input) {
            $input.removeClass('contactErrorBorder');
            $error.fadeOut();
        }

    }



	/*-----------------------------------------
	    	Blog Comments Trick
	 -------------------------------------------*/

    $('.comment').find('ul li').each(function () {
        $(this).parent().parent().children('.first').append('<span class="cline1"></span>');
        $(this).append('<span class="cline2"></span>');
        if ($(this).index() < $(this).parent().children('li').length - 1 && $(this).parent().children('li').length > 1)
            $(this).append('<span class="cline1 cline3"></span>');
    });

	/*-----------------------------------------
	    	Posts Rotator Widget
	 -------------------------------------------*/

    $('.blogPost').find('.tabs').children('li').append('<span class="arrow13"></span>');

    $selTabsPosts = $('.blogPost').find('.tabs').children('li').eq(0);
    $selContentPosts = $('.blogPost').find('.content').children('li').eq(0);
    $contentPosts = $('.blogPost').find('.content').children('li');

    $('.blogPost').find('.tabs').find('a').click(function () {

        $selTabsPosts.removeClass('selected');
        $selTabsPosts = $(this).parent();
        $selTabsPosts.addClass('selected');

        $selContentPosts.stop().fadeOut(150);
        $selContentPosts = $contentPosts.eq($selTabsPosts.index());
        $selContentPosts.stop().delay(150).fadeIn(200);

        return false;

    });


})(jQuery);