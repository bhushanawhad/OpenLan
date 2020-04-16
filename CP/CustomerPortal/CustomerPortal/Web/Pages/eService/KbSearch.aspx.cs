using System;
using System.Collections.Generic;
using System.Linq;
using Site.Library;

namespace Site.Pages.eService
{
	public partial class KbSearch : PortalPage
	{
		private const string _queryParameterName = "query";

		protected void Page_Load(object sender, EventArgs e)
		{
			//RedirectToLoginIfAnonymous();

			var query = Request[_queryParameterName];

			if (Page.IsPostBack || string.IsNullOrEmpty(query))
			{
				return;
			}

			PopulateSearchResults(query);

			if (ResultsList.Items.Count > 0)
			{
				ResultsList.Visible = true;
			}
			else
			{
				NoResults.Visible = true;
			}
		}

		protected void PopulateSearchResults(string terms)
		{
			var searchTerms = terms.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

			var kbarticles = XrmContext.KbArticleSet.Where(k => k.StateCode == (int)Enums.KbArticleState.Published && k.msa_publishtoweb == true);

			var kbarticleResults = Enumerable.Empty<Xrm.KbArticle>();

			foreach (var term in searchTerms)
			{
				var results = IterateSearchTerm(kbarticles, term.Trim());

				if (!kbarticleResults.Any())
				{
					kbarticleResults = results;
				}

				kbarticleResults = kbarticleResults.Union(results, new KbarticleEqualityComparer());
			}

			ResultsList.DataSource = kbarticleResults.ToList();

			ResultsList.DataBind();
		}

		private class KbarticleEqualityComparer : IEqualityComparer<Xrm.KbArticle>
		{
			bool IEqualityComparer<Xrm.KbArticle>.Equals(Xrm.KbArticle x, Xrm.KbArticle y)
			{
				if (x == null && y == null)
				{
					return true;
				}

				if (x == null || y == null)
				{
					return false;
				}

				return x.KbArticleId == y.KbArticleId;
			}

			int IEqualityComparer<Xrm.KbArticle>.GetHashCode(Xrm.KbArticle article)
			{
				if (article == null)
				{
					throw new ArgumentNullException("article");
				}

				return article.KbArticleId.GetHashCode();
			}
		}

		protected IQueryable<Xrm.KbArticle> IterateSearchTerm(IQueryable<Xrm.KbArticle> articles, string term)
		{
			return (articles.Where(k => k.Title.Contains(term) || k.KeyWords.Contains(term) || k.ArticleXml.Contains(term)));
		}

		protected void SearchButton_Click(object sender, EventArgs args)
		{
			var queryUrl = GetUrlForRequiredSiteMarker("KB Search");

			queryUrl.QueryString.Set(_queryParameterName, SearchText.Text);

			Response.Redirect(queryUrl.PathWithQueryString);
		}

		protected string GetKbArticleUrl(object kbarticleid)
		{
			if (kbarticleid == null)
			{
				return null;
			}

			var articleUrl = GetUrlForRequiredSiteMarker("KB Article");

			articleUrl.QueryString.Set("id", kbarticleid.ToString());

			return articleUrl.ToString();
		}
	}
}
