using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class test_Kishan_default : System.Web.UI.Page
{
    public List<package> allPackageData
    {
        get
        {
            if (Session["AllPackageList"] == null)
            {
                Session.Timeout = 1440;
                Session["AllPackageList"] = allPackageList;
            }
            return Session["AllPackageList"] as List<package>;
        }
    }

    List<package> allPackageList = new List<package>();
    List<channel> channelList = new List<channel>();
    EPCHandler epcHandler = new EPCHandler();
    List<package> allData = new List<package>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            allPackageList = epcHandler.GetPackageInfo();
            drpPackage.Items.Add(new ListItem("--Select Category--", "0"));
            bindPackagesDropDown();
            drpLanguage.Items.Add(new ListItem("--Select Language--", "0"));
            bindLanguageDropDown();
            binGridView();
        }
        
        errorMessage.Text = "";
        count.Text = "";
        
        Type cstype = this.GetType();
        StringBuilder callscript = new StringBuilder();
        callscript.Append("<script language='javascript'>Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ddlstyle);</script>");
        ClientScript.RegisterStartupScript(cstype, "callscript", callscript.ToString());
    }
    #region Methods
    public void binGridView()
    {
        allData = allPackageData;
        var orderedPackageList = (from p in allData
                                  orderby p.name
                                  select p).Distinct();
        GridView1.DataSource = orderedPackageList;
        GridView1.DataBind();
    }
    public void bindLanguageDropDown()
    {
        allData = allPackageData;
        var languageList = (from l in allData
                            orderby l.language
                            select l.language).Distinct();
        foreach (var k in languageList)
        {
            drpLanguage.Items.Add(k);
        }
    }
    public void bindPackagesDropDown()
    {
        allData = allPackageData;
        var languageList = (from l in allData
                            orderby l.category
                            select l.category).Distinct();
        foreach (var k in languageList)
        {
            drpPackage.Items.Add(k);
        }
    }
    public List<channel> bindChannelList(string packageId)
    {
        package packageInfo = new package();
        ArrayList channelsInfo = new ArrayList();
        if (packageId != null && packageId != "")
        {
            allData = allPackageData;
            packageInfo = (from item in allData
                           where item.id == packageId
                           select new package
                           {
                               name = item.name,
                               id = item.id,
                               channelList = item.channelList
                           }).AsEnumerable().First();
        }
        List<channel> channelList = new List<channel>();
        foreach (channel x in packageInfo.channelList)
        {
            channelList.Add(x);
        }
        return channelList;
    }
    #endregion

    #region GridView Event
    protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        Label lblPackageID = (Label)GridView1.Rows[e.NewSelectedIndex].FindControl("lblPackageID");
        Label lblPackageName = (Label)GridView1.Rows[e.NewSelectedIndex].FindControl("lblPackageName");
        Label lblPackagePrice = (Label)GridView1.Rows[e.NewSelectedIndex].FindControl("lblPackagePrice");
        lblPackage.Text = "Channel List for Package ID: <u>" + lblPackageID.Text + "</u> | Package Name: <u>" + lblPackageName.Text + "</u> | Monthly Price: <u>" + lblPackagePrice.Text + "</u>";
        try
        {
            GridView2.Visible = true;
            string packageId = lblPackageID.Text;
            ViewState["packageId"] = packageId;
            channelList.Clear();
            channelList = bindChannelList(packageId);
            var orderedChannls = (from item in channelList
                                  orderby item.name
                                  select item).Distinct();
            var query = (from item2 in channelList
                         where item2.number == "0"
                         select item2).Count();
            if (query > 0)
            {
                if (query == 1)
                    count.Text = "we have '" + query + "' channel with 'Channel No. 0'";
                else
                    count.Text = "we have '" + query + "' channels with 'Channel No. 0'";
            }
            else
                count.Text = "";
            if (orderedChannls.Count() == 0)
            {
                GridView2.EmptyDataText = "<strong>Sorry!! There is no channel for this package.</strong>";
            }
            GridView2.DataSource = orderedChannls;
            GridView2.DataBind();
            errorMessage.Text = "";
        }
        catch
        {
            GridView2.Visible = false;
        }
    }
    #endregion
    #region DropDownList Event
    protected void fillLanguage(object sender, EventArgs e)
    {
        errorMessage.Text = string.Empty;
        drpPackage.SelectedIndex = 0;
        string keyword = drpLanguage.SelectedItem.Text;
        if (keyword == "--Select Language--" || keyword == "")
        {
            binGridView();
            GridView2.Visible = false;
            lblPackage.Text = string.Empty;
        }
        else
        {
            allData = allPackageData;
            var filteredLanguageList = (from p in allData
                                        where p.language == keyword
                                        orderby Convert.ToDecimal(p.monthlyPrice) descending
                                        select p).Distinct();
            GridView1.DataSource = filteredLanguageList;
            GridView1.DataBind();
        }
        if (drpPackage.SelectedItem.Text == "--Select Category--" || drpPackage.SelectedItem.Text == "")
        {
            GridView2.Visible = false;
            lblPackage.Text = string.Empty;
        }
    }
    protected void fillPackages(object sender, EventArgs e)
    {
        errorMessage.Text = string.Empty;
        drpLanguage.SelectedIndex = 0;
        string keyword = drpPackage.SelectedItem.Text;
        if (keyword == "--Select Category--" || keyword == "")
        {
            binGridView();
            GridView2.Visible = false;
            lblPackage.Text = string.Empty;
        }
        else
        {
            allData = allPackageData;
            var filteredPackageList = (from p in allData
                                       where p.category == keyword
                                       orderby Convert.ToDecimal(p.monthlyPrice) descending
                                       select p).Distinct();
            GridView1.DataSource = filteredPackageList;
            GridView1.DataBind();
        }
        if (drpLanguage.SelectedItem.Text == "--Select Language--" || drpLanguage.SelectedItem.Text == "")
        {
            GridView2.Visible = false;
            lblPackage.Text = string.Empty;
        }
    }
    #endregion
    protected void findChannels(object sender, EventArgs e)
    {
        try
        {
            errorMessage.Text = string.Empty;
            string packageId = ViewState["packageId"].ToString();
            channelList.Clear();
            channelList = bindChannelList(packageId);
            string channelName = txtChannel.Text.ToUpper();
            var query = (from k in channelList
                         where k.name.ToUpper().StartsWith(channelName)
                         orderby k.name
                         select k).Distinct();
            
            if (query.Count() == 0)
            {
                GridView2.EmptyDataText = "<strong>Sorry!! There is no channel with this name. Please try again with specific channel name.</strong>";
            }
            GridView2.DataSource = query;
            GridView2.DataBind();
            txtChannel.Focus();
        }
        catch
        {
            errorMessage.Text = "please select the package";
        }
    }
}