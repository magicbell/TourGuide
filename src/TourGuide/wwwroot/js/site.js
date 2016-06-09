(function(){

    var $sidebarAndMain = $("#sidebar,#main_container");

$("#sidebar-toggle").on("click", function () {
    $sidebarAndMain.toggleClass("hidden-sidebar");
});

})();