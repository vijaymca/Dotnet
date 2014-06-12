// ---------------------------
// Legal
// ---------------------------
// CastleBusyBox Control
// Castle Rock Software, LLC
// by Mark Wagner
// http://www.crsw.com
//
// Version: 1.2
//
// Copyright 2004, 2005 Castle Rock Software, LLC
// No warranty express or implied.

// ---------------------------
// General Notes
// ---------------------------
// The BusyBox javascript has been written so the BusyBox javascript
// control can be used with or without the Castle.BusyBox .NET control.

// ---------------------------
// Constructor
// ---------------------------
// BusyBox class constructor
// Arguments:
//   id - id of the IFrame tag to use.
//   varName - name of the variable this instance of the busy box is assigned to.
//   imageCount - number of image in the animation sequence.
//   imageNamePrefix - name prefix for each image.
//   imageNameSuffix - name suffix for each image.
//   imageDelay - number of milliseconds to display each image.
//   width - defines the width of the busy box (required for Netscape and Firefox)
//   height - defines the height of the busy box (required for Netscape and Firefox)
//   url - (optional) url to the page containing the custom busy box layout.
//
//   This example uses the default busy box layout defined internally (in the javascript).
//   var busyBox = new BusyBox("BusyBox1", "busyBox", 4, "gears_ani_", ".gif", 125, 147, 206)
//
//   This example uses a custom busy box layout defined in the Busybox.htm file.
//   var busyBox = new BusyBox("BusyBox1", "busyBox", 4, "gears_ani_", ".gif", 125, 147, 206, "Busybox.htm")
//
var intCheckBusyBox = 0;
function BusyBox(id, varName, imageCount, imageNamePrefix, imageNameSuffix, imageDelay, width, height, url)
{
	// Initialize object
	this.id = id;
	this.ImageCount = imageCount;
	this.CurrentImageIndex = 0;
	this.ImageWidth = 0;
	this.ImageHeight = 0;
	this.ImageNamePrefix = imageNamePrefix;
	this.ImageNameSuffix = imageNameSuffix;
	this.ImageDelay = "0";
	this.DivID = "BusyBoxDiv";
	this.ImgID = "BusyBoxImg";
	this.Enabled = true;
	this.Width = "352";
	this.Height = "95";
	
	// Retain the name of the instantiated object variable so that we can animate 
	// using the setTimeout statement
	this.VarName = varName;
	
	// Allows us to stop the animation with clearTimeout(), should we ever want to
	this.timeout_id = null;	
	
	// Url to the page containing the busy box.
	this.BusyBoxUrl = url;
	
	// Get reference to the IFrame object
	this.IFrame = document.getElementById(this.id);
	
	// Hide the busy box
	this.Hide();
	
	if( this.BusyBoxUrl )
	{
		// Load the busy box contents using a custom layout page.	
		this.LoadUrl(this.BusyBoxUrl);
	}
	else
	{		
		// Load the busy box contents using the internally defined layout.
		this.RenderContent();
	}
		
	
	// If this browser does not support IFRAME tags then disable this control.  The
	// next version will implement the use of a DIV instead of the IFRAME tag; 
	// even though there are a couple minor issues with using DIV tags.	
	if( !frames[this.id] )
		this.Enabled = false;
}

// --------------------------------
// Instance Methods
// --------------------------------

// GetIFrameDocument:
// Returns a reference to the document object in the IFrame.
BusyBox.prototype.GetIFrameDocument = function()
{
	var doc;
	
	if( this.IFrame.contentDocument )
		// For NS6
		doc = this.IFrame.contentDocument; 
	else if( this.IFrame.contentWindow ) 
		// For IE5.5 and IE6
		doc = this.IFrame.contentWindow.document;
	else if( this.IFrame.document )
		// For IE5
		doc = this.IFrame.document;
	else
// TODO: Confirm this should be the default
		doc = this.IFrame.document;
	
	return doc;
}

// LoadUrl:
// Changing the src attribute for an IFrame tag causes each new page to be 
// added to the browsers history object.  This causes undesired results for 
// the user when they click the back button.  Instead, we can use the 
// document.location.replace() method to correctly load our busy box 
// page into our IFrame.
//
// Arguments:
//		url - url to the busy box page.
BusyBox.prototype.LoadUrl = function(url)
{
	// Get a reference to the document object in the IFrame
	var IFrameDoc = this.GetIFrameDocument();
	
	// Load the url using the replace method.  This will prevent the browsers 
	// history object from being updated with the new busybox url; thus allowing 
	// the back button to function as desired for the user.
	IFrameDoc.location.replace(url);
}

// RenderContent:
// This method is used when the default busy box layout is used; not a custom 
// layout.  This method is called when the url argument for the constructor is null.
BusyBox.prototype.RenderContent = function()
{
	// Get the IFrame document object
	var doc = this.GetIFrameDocument();
	
	var style = " style='POSITION: absolute;' ";
	
	doc.open();
	doc.writeln("<body ondragstart='return false;' style='Margin: 0px; Background-Color: transparent'>");
	doc.writeln("   <div id='" + this.DivID + "' align=center " + style + ">");
	doc.writeln("      <img id='" + this.ImgID + "' src='/_layouts/dream/images/spacer.gif'>");
	doc.writeln("      <br><h3></h3>");
	doc.writeln("   </div>");
	doc.writeln("</body>");
	doc.close();
}

// Resize:
// Resizes the busy box IFrame by setting its width and height attributes
// to the size of its contents.
BusyBox.prototype.Resize = function()
{
	// Resize the busy box IFrame.
	if( BusyBox.IsBrowserIE() )
	{
		// Set the width by looking at its contents
		var div = frames[this.id].document.getElementById(this.DivID);
		//this.IFrame.style.width = div.offsetWidth;
		//this.IFrame.style.height = div.offsetHeight
		this.IFrame.style.width = "352";
		this.IFrame.style.height = "80";
	}
	else
	{
		// Set the width to the value specified.
		this.IFrame.style.width = this.Width;
		this.IFrame.style.height = this.Height;
	}
}

// Center:
// Centers the busy box IFrame on the page regardless of the browsers
// scroll position.  This ensures the busy box is presented to the user
// in a visible location in the window.
BusyBox.prototype.Center = function()
{
	if( !this.IFrame )
		return;
	
	// Center the BusyBox in the window regardless of the scroll positions
	var objLeft = (document.body.clientWidth - this.IFrame.offsetWidth) / 2;
	var objTop = (document.body.clientHeight - this.IFrame.offsetHeight) / 2;
	objLeft = objLeft + document.body.scrollLeft;
	objTop = objTop + document.body.scrollTop;
	
	// Position object
	this.IFrame.style.position = "absolute";
	this.IFrame.style.top = objTop;	
	this.IFrame.style.left = objLeft;
	
	// Center the BusyBox in the window based on the scroll positions
    var winw = "352"; 
    var winh = "80";
    var documentWidth = (centerElement().offsetWidth)/2+centerElement().scrollLeft-(winw/2);
	var documentHeight = (centerElement().offsetHeight)/2+centerElement().scrollTop-(winh/2);
	this.IFrame.style.top = documentHeight+'px';	
	this.IFrame.style.left = documentWidth+'px';
}
function centerElement(){
return (document.compatMode && document.compatMode!="BackCompat")? document.documentElement : document.body
}

// IsAnimating:
// Returns a boolean value representing the state of the animation.
BusyBox.prototype.IsAnimating = function()
{
	if( this.timeout_id == null)
		return false;
	else
		return true;
}

// IsVisible:
// Returns a boolean value representing the visibility state for the busy box.
BusyBox.prototype.IsVisible = function()
{
	var ifrm = document.getElementById(this.id);
	
	if( ifrm.style.visibility == "visible" && ifrm.style.width > 0 )
		return true;
	else
		return false;
}

// Animate:
// Performs the animation process.  This is accomplished by showing the "current" 
// image in the animation sequence process; and then submitting a timed statement
// to execute in x number of milliseconds.
BusyBox.prototype.Animate = function()
{
	// Assign the current image sequence to display	
		//frames[this.id].document.getElementById(this.ImgID).src = "/_layouts/Images/GEARS_AN.GIF";
		frames[this.id].document.getElementById(this.ImgID).src = "/_layouts/Dream/Busybox.htm";
	
	// Auto re-center and re-size the busy box.  This will force the busy box to 
	// always appear in the center of the window even if the user scrolls.
	this.Resize();
	this.Center();
	
	// Increment the current image index
	this.CurrentImageIndex = (this.CurrentImageIndex + 1)%this.ImageCount;
	
	// Display the next image in (imageDelay value) milliseconds (i.e. 125)
	//this.timeout_id = setTimeout(this.VarName + ".Animate();", this.ImageDelay);
}

// StartAnimation:
// Starts the animation process.
BusyBox.prototype.StartAnimate = function()
{    
    try
    {
	if( this.IsAnimating() )
		return;
	
	this.Animate();
	}
	catch(Ex)
	{
	    return;
	}
}
BusyBox.prototype.StartAnimate1 = function()
{    
    try
    {
	    if( this.IsAnimating() )
		    return;
    	
	    this.Animate();
	    return;
	}
	catch(Ex)
	{
	    return;
	}
}

// StopAnimation:
// Stops the animation process.
BusyBox.prototype.StopAnimate = function()
{    
	clearTimeout(this.timeout_id);
	this.timeout_id = null;
}

// Hide:
// Hides the busy box making it invisible to the user.
BusyBox.prototype.Hide = function()
{	
	this.StopAnimate();	
	// Hide the busy box.
	this.IFrame.style.visibility = "hidden";
	this.IFrame.style.width = 0;
	this.IFrame.style.height = 0;
	//this.IFrame.src = "/_layouts/Images/GEARS_AN.GIF";
	//this.IFrame.src = "/_layouts/dream/Busybox.htm";	
}

// Show:
// This function displays the busy box to the user.  This function centers the 
// busy dialog box, makes it visible, and starts the animation.  This function 
// will typically be called by the body event.
//
// Example:
//		<body onbeforeunload="busyBox.Show();" >
BusyBox.prototype.Show = function()
{	
	if( !this.Enabled )
		return;

	//if( this.IsAnimating() || this.IsVisible() )
		//return;	
	
	this.Resize();
	this.Center();
	
	// Set the busy box to be visible and make sure it is on top of all other controls.	
	this.IFrame.style.visibility = "visible";
	this.IFrame.style.zIndex = "999";	
	// Start the animation
	this.StartAnimate();
}

// --------------------------------
// Class Methods
// --------------------------------

// IsBrowserIE:
// Returns true if the executing browser it a Microsoft Internet Explorer browser.
BusyBox.IsBrowserIE = function()
{
	try
	{ return (window.navigator.userAgent.indexOf("MSIE ") > 0); }
	catch(x)
	{ return false; }
}

// IsBrowserNS:
// Returns true if the executing browser it a Netscape browser.
BusyBox.IsBrowserNS = function()
{
	try
	{ return (window.navigator.userAgent.indexOf("Netscape") > 0); }
	catch(x)
	{ return false; }
}

// IsBrowserFirefox:
// Returns true if the executing browser it a Firefox browser.
BusyBox.IsBrowserFirefox = function()
{
	try
	{ return (window.navigator.userAgent.indexOf("Firefox") > 0); }
	catch(x)
	{ return false; }
}