$('.toggle_login').on('click', function() {
  $('.container_login').stop().addClass('active');
});

$('.close_login').on('click', function() {
  $('.container_login').stop().removeClass('active');
});