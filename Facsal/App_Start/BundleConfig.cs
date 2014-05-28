﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Facsal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(
                new StyleBundle("~/Content/css")
                  .Include("~/Content/durandal.css")
                  .Include("~/Content/toastr.css")
                  .Include("~/Content/Site.css")
            );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-{version}.js"));

            bundles.Add(
                new ScriptBundle("~/Scripts/vendor")
                  .Include("~/Scripts/jquery-{version}.js")
                  .Include("~/Scripts/knockout-{version}.js")
                  .Include("~/Scripts/knockout.validation.js")
                  .Include("~/Scripts/Q.js")
                  .Include("~/Scripts/chrisjsherm/chrisjsherm.*")
                  .Include("~/Scripts/breeze.debug.js")
                  .Include("~/Scripts/fastclick.js")
                  .Include("~/Scripts/foundation/foundation.js")
                  .Include("~/Scripts/foundation/foundation.*")
                  .Include("~/Scripts/jquery.inputmask/jquery.inputmask-{version}.js")
                  .Include("~/Scripts/jquery.inputmask/jquery.inputmask.extensions-{version}.js")
                  .Include("~/Scripts/jquery.inputmask/jquery.inputmask.date.extensions-{version}.js")
                  .Include("~/Scripts/jquery.inputmask/jquery.inputmask.numeric.extensions-{version}.js")
                  .Include("~/Scripts/moment.js")
                  .Include("~/Scripts/toastr.js")
            );

            bundles.Add(
                new ScriptBundle("~/Scripts/main-built")
                    .Include("~/App/main-built.js")
            );
        }

        private static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
            {
                throw new ArgumentNullException("BundleConfig ignore list.");
            }

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            //ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}