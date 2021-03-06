/*
 *  Copyright (c) 2015, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

// For clarity, this sample has all code in the one file. In a real project, you'd put every
// class in a separate file.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using React.Sample.CoreMvc.Models;
using React.Sample.CoreMvc.ViewModels;

namespace React.Sample.CoreMvc.Models
{
	public class AuthorModel
	{
		public string Name { get; set; }
		public string GithubUsername { get; set; }
	}
	public class CommentModel
	{
		public AuthorModel Author { get; set; }
		public int Id { get; set; }
		public string Text { get; set; }
	}
}

namespace React.Sample.CoreMvc.ViewModels
{
	public class IndexViewModel
	{
		public IEnumerable<CommentModel> Comments { get; set; }
		public int CommentsPerPage { get; set; }
	}
}

namespace React.Sample.CoreMvc.Controllers
{
	public class HomeController : Controller
	{
		private const int COMMENTS_PER_PAGE = 3;

		private readonly IList<CommentModel> _comments;

		public HomeController()
		{
			// In reality, you would use a repository or something for fetching data
			// For clarity, we'll just use a hard-coded list.
			IDictionary<string, AuthorModel> authors = new Dictionary<string, AuthorModel>
			{
				{"daniel", new AuthorModel { Name = "Daniel Lo Nigro", GithubUsername = "Daniel15" }},
				{"vjeux", new AuthorModel { Name = "Christopher Chedeau", GithubUsername = "vjeux" }},
				{"cpojer", new AuthorModel { Name = "Christoph Pojer", GithubUsername = "cpojer" }},
				{"jordwalke", new AuthorModel { Name = "Jordan Walke", GithubUsername = "jordwalke" }},
				{"zpao", new AuthorModel { Name = "Paul O'Shannessy", GithubUsername = "zpao" }},
			};
			_comments = new List<CommentModel>
			{
				new CommentModel { Id = 1, Author = authors["daniel"], Text = "First!!!!111!" },
				new CommentModel { Id = 2, Author = authors["zpao"], Text = "React is awesome!" },
				new CommentModel { Id = 3, Author = authors["cpojer"], Text = "Awesome!" },
				new CommentModel { Id = 4, Author = authors["vjeux"], Text = "Hello World" },
				new CommentModel { Id = 5, Author = authors["daniel"], Text = "Foo" },
				new CommentModel { Id = 6, Author = authors["daniel"], Text = "Bar" },
				new CommentModel { Id = 7, Author = authors["daniel"], Text = "FooBarBaz" },
			};
		}

		public IActionResult Index()
		{
			return View(new IndexViewModel
			{
				Comments = _comments.Take(COMMENTS_PER_PAGE),
				CommentsPerPage = COMMENTS_PER_PAGE
			});
		}

		public IActionResult Comments(int page)
		{
			var comments = _comments.Skip((page - 1) * COMMENTS_PER_PAGE).Take(COMMENTS_PER_PAGE);
			var hasMore = page * COMMENTS_PER_PAGE < _comments.Count;

			return Json(new
			{
				comments,
				hasMore
			});
		}
	}
}
