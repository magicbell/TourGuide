(function(){

    var $sidebarAndMain = $("#sidebar,#main_container");

$("#sidebar_toggle").on("click", function () {
    $sidebarAndMain.toggleClass("hidden-sidebar");
});

})();