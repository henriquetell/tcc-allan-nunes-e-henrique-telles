
function ToggleMenu() {
    $("#sidemenu").toggle()
}



function menuFixed() {

    $(window).bind('scroll', function () {
        if ($(window).scrollTop() > 50) {
            $('.wrapper-header').addClass('fixed');
        } else {
            $('.wrapper-header').removeClass('fixed');
        }
    });
}


function initCarouselFaculdades() {
    $('#carousel-faculdades').slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 3,
        dots: false,
        centerMode: true,
        arrows: false,
        centerPadding: '30px',
    });
}

function initCarouselDepoimentos() {
    $('#carousel-depoimentos').slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 3,
        arrows: true,
        variableWidth: false,
        autoplay: false,
        autoplaySpeed: 5000,
        centerMode: false,
        responsive: [
            {
                breakpoint: 480,
                settings: {
                    arrows: false,
                    centerMode: true,
                    slidesToShow: 1
                }
            }
        ]
    });
}

function initCarouselAjuda() {
    $('#carousel-ajuda').slick({
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 1,
        arrows: false,
        autoplay: true,
        autoplaySpeed: 5000,
        centerMode: false,
        responsive: [
            {
                breakpoint: 480,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 1,
                    slidesToShow: 1,
                }
            }
        ]
    });
}


function initCarouselDepoimentoFalas() {
    $('#carousel-depoimentos-falas').slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        variableWidth: true,
        autoplay: false,
        autoplaySpeed: 5000,
        dots: true,
        arrows: false,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    arrows: false,
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    centerMode: true,
                    centerPadding: '40px',
                }
            }
        ]
    });
}

function initCarousel() {
    $('#carrosel-cursos-sugeridos').slick({
        infinite: false,
        slidesToShow: 3,
        slidesToScroll: 1,
        variableWidth: true,
        autoplay: false,
        autoplaySpeed: 5000,
        dots: true,
        arrows: false,
        responsive: [
            {
                breakpoint: 600,
                settings: {
                    arrows: false,
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    centerMode: true,
                    centerPadding: '40px',
                }
            }
        ]
    });
}






function setMenuActive() {
    $('.menu li').click(function () {
        $(this).siblings('li').removeClass('active');
        $(this).addClass('active');
    });
}



function scrollDiv(div) {
    setMenuActive();

    $('html, body').animate({
        scrollTop: $('.' + div).offset().top
    }, 1000);
}



function scrollDivMobile(div) {

    ToggleMenu();

    setMenuActive();

    $('html, body').animate({
        scrollTop: $('.' + div).offset().top
    }, 1000);
}


function init() {
    initCarouselFaculdades();
    initCarouselDepoimentos();
    initCarouselDepoimentoFalas();
    initCarouselAjuda();
    initCarousel();
    menuFixed();
    $('.telefone').mask('(00) 00000-0000');
}



// FUNCAO DE INICAR PAGINA
init();