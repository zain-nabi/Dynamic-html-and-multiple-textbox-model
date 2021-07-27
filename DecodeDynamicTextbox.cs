[HttpPost]
        public async Task<IActionResult> Edit(RepTargetViewModel model)
        {
            var RepTargetViewModel = new RepTargetViewModel
            {
                RepTargetsNewBizList =
                    JsonConvert.DeserializeObject<List<RepTargetsNewBiz>>(WebUtility.UrlDecode(model.MonthlyTargets) ??
                                                                    string.Empty),

                RepTargetsNewBizListCompare = await RepTargetService.GetRepTargetNewBiz(model.FinancialYear, model.RevRepCode, model.BranchID)
            };

            for (int i = 0; i < RepTargetViewModel.RepTargetsNewBizList.Count; i++)
            {
                for (var x = 0; x < RepTargetViewModel.RepTargetsNewBizListCompare.Count; x++)
                {
                    if (RepTargetViewModel.RepTargetsNewBizList[x].Target == null)
                        RepTargetViewModel.RepTargetsNewBizList[x].Target = 0;
                    if (!string.Equals(RepTargetViewModel.RepTargetsNewBizList[x].Target.ToString(), RepTargetViewModel.RepTargetsNewBizListCompare[x].Target.ToString()))
                    {
                        var response = await RepTargetService.GetRepTargetsNewBizById(RepTargetViewModel.RepTargetsNewBizListCompare[x].RepTargetsNewBizID);
                        response.Target = RepTargetViewModel.RepTargetsNewBizList[x].Target;
                        await RepTargetService.UpdateRepTargetNewBizAsync(response);
                    }
                }
            }
            string redirectUrl = string.Format("/RepTargets/Index?FinancialYear={0}", model.FinancialYear);
            return RedirectToAction("Message", "Home", new { type = Service.Utils.StringHelper.Types.UpdateSuccess, url = redirectUrl });
        }
