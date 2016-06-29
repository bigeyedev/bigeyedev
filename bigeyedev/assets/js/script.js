$(function(){
	$('.valid-number').bind('keypress', function(e) { 
		return ( e.which!=8 && e.which!=0 && (e.which<48 || e.which>57)) ? false : true ;
	 })
	
	$('.fancybox').fancybox({
		padding: 0
	});
	
	
	$('.datepicker').datepicker({
		format: 'yyyy-mm-dd'	
	});
	$('.timepicker').timepicker({
		showMeridian: false,
		minuteStep: 1
	});
	
	
});