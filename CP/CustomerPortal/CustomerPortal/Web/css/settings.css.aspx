<%@ Page Language="C#" ContentType="text/css" %>
<%@ OutputCache Duration="600" VaryByContentEncoding="gzip;deflate" VaryByParam="None" %>

html, body {
	background: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/body_background %>" />;
}

a {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/links %>" />;
}

a:hover{
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/links_hover %>" />;
}

a:active {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/links_active %>" />;
}

a:visited {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/links_visited %>" />;
}

#layout {
	width: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/layout_width %>" />;
}

#hd {
	background: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/header_background %>" />;
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/header_txt_color %>" />;
}

#hd a {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/header_links %>" />;
}

#hd a:hover {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/header_links_hover %>" />;
}

#hd a:active {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/header_links_active %>" />;
}

#hd a:visited {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/header_links_visited %>" />;
}

#hd .navigation {
	background-color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/navigation_bar_background, transparent %>" />;
}

#hd .navigation li a {
	background-color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/navigation_background %>" />;
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/navigation_font %>" />;
}

#hd .navigation li a:hover {
	background: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/navigation_background_hover %>" />;
}

#main {
	background: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/left_column_background %>" />;
	width: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/left_column_width %>" />;
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/left_column_txt_color %>" />;
}

#bd .breadcrumbs {
	background: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/breadcrumbs_background %>" />;
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/breadcrumbs_txt_color %>" />;
}

#bd .breadcrumbs a {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/breadcrumbs_links %>" />;
}

#bd .breadcrumbs a:hover {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/breadcrumbs_links_hover %>" />;
}

#bd .breadcrumbs a:active {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/breadcrumbs_links_active %>" />;
}

#bd .breadcrumbs a:visited {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/breadcrumbs_links_visted %>" />;
}

.pageTitle {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/pageTitle_font_color %>" />;
}

#sidebar {
	background: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/right_column_background %>" />;
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/right_column_txt_color %>" />;
	margin-right: -<asp:Literal runat="server" Text="<%$ SiteSetting: /css/left_column_width %>" />;
	width: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/right_column_width %>" />;
}

#sidebar a {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/sidebar_links %>" />;
}

#sidebar a:hover {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/sidebar_links_hover %>" />;
}

#sidebar a:active {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/sidebar_links_active %>" />;
}

#sidebar a:visited {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/sidebar_links_visited %>" />;
}

#ft {
	background: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/footer_background %>" />;
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/footer_txt_color %>" />;
}

#ft a {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/footer_links %>" />;
}

#ft a:hover {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/footer_links_hover %>" />;
}

#ft a:active {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/footer_links_active %>" />;
}

#ft a:visited {
	color: <asp:Literal runat="server" Text="<%$ SiteSetting: /css/footer_links_visited %>" />;
}
