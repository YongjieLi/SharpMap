using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using GeoAPI.Geometries;
using SharpMap.Converters.Geometries;

public partial class Simple : System.Web.UI.Page
{
	private SharpMap.Map myMap;

	protected void Page_Load(object sender, EventArgs e)
	{
		//Set up the map. We use the method in the App_Code folder for initializing the map
		myMap = MapHelper.InitializeMap(new System.Drawing.Size((int)imgMap.Width.Value,(int)imgMap.Height.Value));
		if (Page.IsPostBack) 
		{
			//Page is post back. Restore center and zoom-values from viewstate
			myMap.Center = (ICoordinate)ViewState["mapCenter"];
			myMap.Zoom = (double)ViewState["mapZoom"];

		}
		else
		{
			//This is the initial view of the map. Zoom to the extents of the map:
			//myMap.ZoomToExtents();
			//or center on 0,0 and zoom to full earth (360 degrees)
			//myMap.Center = new SharpMap.Geometries.Point(0,0);
			//myMap.Zoom = 360;
			//Create the map
			GenerateMap();
		}
	}
  
	protected void imgMap_Click(object sender, ImageClickEventArgs e)
	{
		//Set center of the map to where the client clicked
		myMap.Center = myMap.ImageToWorld(new System.Drawing.Point(e.X, e.Y));
		//Set zoom value if any of the zoom tools were selected
		if (rblMapTools.SelectedValue == "0") //Zoom in
			myMap.Zoom = myMap.Zoom * 0.5;
		else if (rblMapTools.SelectedValue == "1") //Zoom out
			myMap.Zoom = myMap.Zoom * 2;
		//Create the map
		GenerateMap();
	}
  
	/// <summary>
	/// Creates the map, inserts it into the cache and sets the ImageButton Url
	/// </summary>
	private void GenerateMap()
	{
		//Save the current mapcenter and zoom in the viewstate
		ViewState.Add("mapCenter", myMap.Center);
		ViewState.Add("mapZoom", myMap.Zoom);
		//Render map
		System.Drawing.Image img = myMap.GetMap();
		string imgID = SharpMap.Web.Caching.InsertIntoCache(1, img);
		imgMap.ImageUrl = "getmap.aspx?ID=" + HttpUtility.UrlEncode(imgID);
	}
}