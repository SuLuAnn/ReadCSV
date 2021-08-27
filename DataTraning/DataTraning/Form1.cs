using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTraning
{
    public partial class Form1 : Form
    {
        private StockDBEntities StockDB;

        private StockVote StockVote;

        private FundNoBusinessDay FundNoBusinessDay;


        public Form1()
        {
            InitializeComponent();
            StockDB = new StockDBEntities();
            StockVote = new StockVote(StockDB);
            FundNoBusinessDay = new FundNoBusinessDay(StockDB, TimeText);

        }

        private void ClickVoteDayAddButton(object sender, EventArgs e)
        {
            StockVote.AddReviseStockDetail();
        }

        private void ClickVoteDayDeleteButton(object sender, EventArgs e)
        {
            StockDB.股東會投票日明細_luann.RemoveRange(StockDB.股東會投票日明細_luann);
            StockDB.SaveChanges();
        }

        private void ClickVoteDataAddButton(object sender, EventArgs e)
        {
            StockVote.AddReviseStockData();
        }

        private void ClickVoteDataDeleteButton(object sender, EventArgs e)
        {
            StockDB.股東會投票資料表_luann.RemoveRange(StockDB.股東會投票資料表_luann);
            StockDB.SaveChanges();
        }

        private void ClickFundDetailAddButton(object sender, EventArgs e)
        {
            //string a = "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=EOD0S61qjKbxajuFt8r0WLS70VypV%2B00iExDRe5KrPn3CNm8qjSStf6O8ojSP9u2NfACj7%2BwEWJofVdWk%2B%2BzrfegT7LySYd%2BUHrWETAZK5FaTuQC28lj6SaM2uBOrB6OvagVdP6IKplCOg6WXDN%2BZhyzww19ONLbJq74m0F8oIv4W1HgbeJdQiDvMHscHGOhPkBEbuUenROE7FGo1qk3843009JuAghQMds7W0ctGUXIjAmWvk9ZNNn6TFvFjMCkhW6N3YbYw%2Bd4YdSAGaFeo4VkPuA0O8RaX%2Fz%2Fv59IsLoTd0ijuqF35aMaUY1ufYQop8n0UZ%2FO1mgxOS%2FzLwBLZwqMtM%2BYk0oL5Mpj%2BjIyiKTARmw8RkVSuT%2FFw%2FGqGxzJ44OlQnvRcs5mSFmO2Jx50oUV8d5E4jTCwvXC291nWD4k1un6T2BONEMcMa8a1o7FhHE7DmD32DmXVJq%2FB%2FnEylhGSLuHrn%2FYJYT6INNgtMZibta4RrkkreOTOnRedUl8DWNiDM7RtKcH5h7DOA4Kl%2FbexqTIdWPL1AIVjG4erbzYW%2B%2BC%2FMdNbGcqvEd8MX8w1Gzsdw50ja62Bk8M8N48BXVzTaTVjMXYntQHPKw%2BaAm67DqAQKXbxmdSt9Cp%2BvLYYle2x1Gu0oszKIJjZgFYpJpXoPx%2BPErX00nJsGDq6ovezfkxGAGEeQj5pxJbuEOvaqZVCeQxZUZtIKv9CKjEIt%2Fcbs9aHviSGfRsmR6v%2BLZuWV1ZY7gpfnffrP4PJhY0%2FMVCvjoocpXCNRbGF%2F%2BNR7m5iux8lOQ8GxdVPvznSarlpMTuz4SagFx7CmRgS%2BqUJW3utjfSeY2%2B0nTfJgazwg0tetx5WxbAImoGZVozrMA1YjzJoCZ%2FJNIZezLoc2IykTdQun7UNoCAUyEqkbEF8diC4OLY6SDLGTvOQFF7ZGz2%2BlXS%2FogBXBAP7V7MChq717Ksdjzyh94qIuOY%2BG%2BK62BWn7XF3WfGCFIQBU8Rl2C1RYeJ1IuYG%2BbBOchD8J1VRkC8I2ejKNerQsG4lxb9C0rZvV49T9hrNCP4XX69Chv6WQiVL9D%2Fx3oXszYcycxJ%2F6%2BJD%2B6oWjh%2BLZObnC%2FYP6Tx4R1j7sNiitpssQcss%2BTn1kMqmVNLCMAknLOd0t8cNjxg3Im4NClW%2FXkGLfYVd883mtToenrn%2Bm2OrgA%2FptXGTtAR88zKUfKqq2l06DlYTy1dnCi3lsYZ531ysmVZcug3WFA6l2mVHQrXBcy1ghSh9k2FiH%2B91EvaFBXYupCSXuwMlSzx0OwcQKsTdHYwpaBVrnxUSNOA48E%2FyRYT3uvj1OuScBhOiLq7d980FDYMPp9qHRf7k2CvdWh3SH%2FlJbuupbaDqCqr7wc1nDvzr4uIAIYCqxemBVzPwgluB4DWivxCmvN9LWjDocwW6fGxAU9Olv9Os8NrRucAQtm2U7x9sg%2B%2B4%2B1f7cT8MPB6sPQL%2FdKfPKI6ufXg2Or%2F2x1ZYrgxJr6gqAMP%2FFboQ5dP5c4c7DbSX78YV3WoMJgGfWc6aJuQvUhnsvCmxPPRgbzAwRNi6uxK2Lm4Pjgw%2BFpCqA2IiPmWkJpdlT7%2BYIGb6vMm40ANS%2F5DoKCrWP3AVwGDGLSvs1382cEHPcLtv8XuxrAszLhLR9mRKws6fWVRfmCYHg%2FhCznyfVDL1JdbjcuXfr%2FXzi%2BzW%2FtmecKvFdZWw5fQXjY1RmoSbeokD3dZwvG7IwJuHC3BNAzIV%2FD88QMp7q6SvqtGiWIRHWWOr0a8OMUaU4sGzt1kzcjFk4kJKCNqUKMkVlAo0D8J12VrDv2XAiBaeAGkBTrAtROKXNqGJXToqm8blPo4YPgJ3njIFjjQGlvvQ7gMLPe9cydgYXLCOW%2BYR1TOcV%2BaxJMsMxIpEI7i%2B3RTL2y6Nm9xLCSQ%2FOjMg77kSzqf%2FQC5oT%2FAS92hjwmJWsdS%2FAI4e%2BRJZnMMiBfFhXAN%2FKydeAvVbN1Y6m1LTfbKNIwj4Zqi2r1mUmN5h8vL%2BLjMuVLOxDaKgYgdwfldS7oV3E6srwh0MfcXGCShw98VV%2BG8RbmBpXbeRfq1K%2FTkYqvLAf%2B4qbXY27aJur9AaVxaK5rqEb7PgK%2FsWH2BehxFvwoxLja0X5kDJROJWXtlQ0%2Fl3yrOnboNMvvrAf94wlYz1HOSy%2BO%2F4oY%2BVnI%2BvqRPqDw4TFhO%2Fvf4FsbxJtK8PI1sVv%2FUvEP5MIhrmy3B0kEFTK9%2BOkE%2B7VLL8BviPfDVhZ3lBp2IKKhoiefEGNKHsQuboETA9pWkrw5cgkX9TRYYjs%2BEwFMnE0ikXA6e0%2FPxXcvhf2Y9uqARVNu%2BAkqjjE%2FZOgVdzKSXZUhMzOKBKtl5HBFLfd8oDnAp0CscndQJpYJ3pljJSLA%2BgEerywpPamEilryvo4MUKnb%2BXFHo1Xw0lkwYnJQcJ%2BQuKsXWITtQIAXcJXLwOij7MfMNAP%2FOOLy7VnYs99XkbyQzd5k7nDkvfE0fujCNE1b0eKkDbRrtnIEOjCAPfZL2icpVn0rg6Vz5bvCbkst%2Bnak9KwrLzg9dL%2B8%2FYq17uancP0ATmp%2FtEIpLPbpQpH2LfSctngGX7mazcBHbVSGT1o%2F%2BAzYWK4B%2Btr6gNllNUnHPZsxXySDDjC%2BNv4McZdsf12lefUJQOCNX%2BphekrRNlUBfv7SNG%2F1exjhu%2FKabjJMQA5zqiUSvkoJQ2jeMQk%2BDWEm8b9JJA8OwkhpHUlQaLFy9L%2B4igp4X3nksF539xM%2Fw9NMpY8eqbtUyvJ3OuxKOgrKx76LnwcExRwPS6bQdPmNUb1no5cq6hWooB70fP6p%2F6jKjiXQRrbpLWWwIL2Ec%2BMUc4ddxUopHn8fqGuSl2J99kmXn8z0LOtV3qLx2nAgwlDG7%2FQJlc%2BYW1Yn1AuzWe1aPJ00RStqN3JBo3%2BJITrKIvLZd%2FhiorFH1CCz%2B2Ph6sYy2npoDakKl%2FMGOwkNFCkQdmra8Nf3itaQ1Gzwy45hoYM5ufslQg2Ydk2sYaZAUIkaSEJKdLnsN2yVTZ0qDgQRYCdOzz1XSyMwatEva6o8tfXZEYla1d%2F7oFVbrUqbrzYkzzyz7zSjxTons7Fo1GdAF%2FP8tjmx2qqoOi14B%2BZykMJlPFnKP7li8SXeHDzhay7Q8x%2BXrSD92HfG5BcMrdVrT4ZwTtFSmAJM6BtsQHk0B3BuoE4P5mpWOP2OXSjfPpAjiJ%2FXJ%2BDYrVRu0gwjyYkgAP1kJhe3gz1WIq1AtJI1lSDIP2LJuDX%2B3Xg8Bed5E8Ifvoe%2FYidtLqoNHlOQJQrz2CjVt3DGKODvdHfd056cPAgH5UiZko5i2Q4uD7t5I4g27Q%2BrZ5Hxk6NtNVrQUmBm6nTN9roKoQv59%2BFKv9c1GfsAkGb%2Fi1X5pI2KZF2GJd4UlzX7Wb%2FYjv1GN53k4ngH8kguqf8QmwqkGFWZ%2FOJ4U28sSgOGzy06CsPz8MIPZMpSovN84LOsoX122POlJcstoG2y5TR27To8xs0DVk4WC8u8DE8ouOvbSnUm38QMcg1eQde%2Fdg3z0I77nHqRLEoQXsPXg03cPm4tl%2BSrFr0hTRNBgJp5vqAZ2ykrLZRufjdPnwYARwAYvdR9fWYeUkzxB9ROmm61B%2FBAW5VA8b0%2Fpd5lULY7eNAIS6PC9ZiLBL2E1k%2F5Z1RwCEwXX%2B3fnv2nNI7e%2FeEfga18Tv76kzVOZLA9Ei%2BrYcNjqZheQhA4sk4fT2awE7v6itrVBmhB%2BDnuN2muzaopj22OWeNumidJn3MlELIRoM6MRWsu5F7EVsNQFNiSdF0s141tqFaUNbuvyy3mUPLMSiArtnYbP2MeQ3qS9nGCH2Xucp1g6l6XLbBe81ulCFLyEnUCkaoznsvRmDnsXvGxuKz6rWLZULFwAQBdLGJSj0teMrPZ4vKUTzkG3C%2FO9XOB1IM46t3nCEsqfVTu1PiXMY%2B2XCSe%2BgPjJcJM2AGclV2L8x3LMv1TpboVvB1yNz46H25o3NHsffSKZFOY7E4MoWAb52iajw6Jo8vXbstMsRxrt0MT84b1ywkSg92uxrr%2BfutfyIZJ7vetg9TP%2F%2F61EnHZFo%2BwPD0bk52AotbcktCx%2B5%2F3LVV%2BoJB57qmVhxLocE1DBUWZOTXb45a1YE%2FEIj1eTB2c2davHPlTghLnhQ2UPd2Z5lvq04NJDzZvztfcov7D6jllmgWAfdLuGW4uIAUl30%2FNemRQDypiROOOH9NsuX%2BC74mPN%2FY3xfkQix98sZHVrVWfiRWcQ6pV5wQoxyqM2J6fGPqAKibxWY73FmLStauDS1VkDoYG0nFiN4sv3kV9FH1Tv13gOzb7FLvH6sOuptoqLCTN0MLG2nOGpUEKi8HBJwJ6MIo5Fisp2TukTy5oElCozO08Kbf9IpZPkQAs3EnlUb5K3A%2BqYK%2FCRpLjzdATfaA9CQXOTj8GNCSWNFXGta6TZiGJOlmbCt%2BQb%2BY6%2BTxw%2FH0qnlDtuZ7x9QGfpA9euSkvVxapxzNh%2FaOCiP%2BhgbxH6HJhpxYfk35sKsK6ittFn5UlqNAB8V8CAnicfxkCU42xqw0iWPuRalIf5IAUMVtRQyq%2BNOxfDQH%2BoWJKeBnSHeG%2Bi97bQ6M6tHsb8hpWZrWyv1nn1QQCnT4HMTC7g7xlQAcNgz9kDMSnsr5lBDLvnw9KBok1P%2Bb4maAyx7uiSE0WoPAwRZClCEqTu2yqsdlyZ9Prlc0odSUP%2B8Ml3BcTbN%2FgslQU%3D&__VIEWSTATEGENERATOR=84ED556B&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=PMfd%2Fgk2KbbBsPDuvZE8esrlPB5AhECESCZt%2Bru2SLiKQavwmrhyzGpRki8Wq1TgiqGkzTF%2FBj3%2FjHdHPJACj1ai3oE3B6FBXxHIT%2BPMkP8RL1OCrDY0fORbZ8nJz4uW%2F87httnCywEsEcooPMYYjvUTRipy%2B5wqh5ZgmgGviMqFcr1LROy2xe5Nw0n9f4yF8hQjhAFUK9RyWFbmHX0MdNCGc%2F1cF6SPrQDPB5D4rPSChUtmEsUBHFNn0Y%2FqL2e6xO1%2BF70Qcl3d2JGz41B%2F862KepPx2kFpIhjpZHOsAhVWcnsXb73jCYjlmIzNlC9krshqo1s%2BA6Br9DnF%2B0rCbPrSgdibZI5CMWTkbirsFnXXDWOps%2F2eTVXx9wsdtcBnvpBmk1IWzsuynbnYDBh9ben7zdVHvnd8dkaZprAoJfGchMW8xxSVDtxh0QL%2B8BdPOORvsfRN5Lj32HZ9rZQ66IYMHuhP1VvJX7GsdXZXvt944cUU3r4qxiSEZriUbFHrQe4dpAZl3d6d2I2zKgNTVEYeRF%2BSf6wUWTFR%2FAJ5j82kSd8nF50sNpkkL3Fzvb%2BjjHSWkwuMVm6VRZ68KAP5qAoaI04AvxDLPxuSP3k1wzhRXB4th7vgxfiV4kh5AWODHwtj9mKKgpz9x9d7YSA9KF2arNiYeMygW68tdg4U3TCCF1Kse4kNP5GFmkOy%2BZ0VG%2FIRhgo85ozdL534PQwxiNVnYpdNcvHqpMKkLcA7Q3QbK323gVuTlWsDXZJ6mAZG30%2F2CBxUHKbOrdD9RSWWvJzxLHcdacmRQ%2BsoNlvmYcrxTcHgApYFTC8nnsYyMVDZOMNqSXhjEER5PKKbVmRvJEOqXD5f6sPVN0prea6mZNCo%2FST5w%2FjZlmSD3BMzvIhtvaoYfAPUE6MB5lqtQoci6WGoGN228ZIPoYSn8h7fFyYu2kkyxLIMrcJ%2F8xdsksbAVziVgF511gbcr9eliCpYG%2FT3IXOuvjdTiL7m9Y9Qk1Yih4FI9ywD1i84W8hwKtCZFjcxAnaFM5N0lRbHXeayWb5e%2BjVtxhKezg6TFZYGMV3D8TLs3ugRU66j7WK5IWDVPQj6jp9ryZOq5OsErMZbJgX3Jc5EqK7MrrsEPztCnsNiEsD0DNREHjTojalnJvwaMzMLS9hVNy7Di4VsYfTALBePUUZiWCrZsMvEP%2BxJvSbQDismOpK4X4sHLOSsA9cbBKWvqxItVKJDjq1zvI4pfpO2fsHlIniUkrbDjTvw9MBMyRE3wr6uPVg%2Ff%2BQFRV2y3fUBLbD4I8S2vP0RFGrVxQbpTAvbNnCIzZ%2Fngspotpsptdcyenz4rekwLrVDMc7vJK5PpkmHEdFGC6uN1aaF5eGxInFvpp9m7vLOzrK52veLYHyQuyJ8PFbXKAEadjj6wgTAs5B6fKrg3l7ZVCZAZcJKtq0WZbA42S0BQdSc12XPzTXKHOLlTxkIGB6QQ8SWS%2BFEr14v%2BK6ZfIn3s7ygCkL7xo9s8xaWlaaZTZFBFSRqyhN4woJsnUckyXhPPBLqpp2KHkWm9ftBMZEUmOq3E%2FRvLClqORW0Y5o2iPy2MA9UPdG30l%2B0KgiHNGkC4SE6WxDSrSCUQvNZAG7XGiOS5A3p6ftLy7NSNplVx1w1WRvGSRYDqyClUmi4pEH0hiFdmcOnnC4TiIb84MNHA909HAsjRd7TkL7KSl%2BIV6j5%2F%2FD4M%2FEhl8dF0uoAw10n3vywluK4Z6UiL4r15MZ4lT%2F5gJFxsMqcvN922%2F5lzjiOY1wejDpPw%2F5Q40J2eYk1H0YpDRaahXV6dkCBesJxkvqTtmcbH4%2Bjrol88n9pndKK2waUQkbBbsbsbzstt590tzGRHk1kAA%2B4n6BJyNrXSCcsatDsOraH8ZcJDdcGOPMXs2CmxrBcuuiZZmJIxtkkyoMo%2BFtvdYNKxmEcNqtkw81CCIeanDe9BYOJfq7HK%2F870DE9c00A7QPDQzVeCvdj4XVV0Hxf1PQ2c994zAJTP7s%2F8hJUmOjfky8euxtEOoyoCjHl2fdBZMPqZzCcHBO4oHl6p3CUAg3U%2FCn6OVhLiPVz0sDwul3ApzhVEDxELFP0FB8VWzgBPCzpBRmz60zQ%2FHVeA9wncPkM7ECZgr6JRaA7TnpMdbhk6bT2Fgq%2B9O2p9djlaBofu24K5qpry0Jcito5&ctl00%24ContentPlaceHolder1%24ddlQ_Year=2021&ctl00%24ContentPlaceHolder1%24ddlQ_Comid=A0003&ctl00%24ContentPlaceHolder1%24ddlQ_Fund=&ctl00%24ContentPlaceHolder1%24ddlQ_PAGESIZE=&ctl00%24ContentPlaceHolder1%24BtnQuery=%E6%9F%A5%E8%A9%A2";
            //webBrowser1.Navigate(Global.FUND_NO_BUSINESS_DAY, string.Empty, Encoding.UTF8.GetBytes(a), "Content-Type: application/x-www-form-urlencoded");
            FundNoBusinessDay.AddReviseFundDetail();
        }

        private void ClickFundDetailDeleteButton(object sender, EventArgs e)
        {
            StockDB.基金非營業日明細_luann.RemoveRange(StockDB.基金非營業日明細_luann);
            StockDB.SaveChanges();
        }

        private void ClickFuturesPriceAddButton(object sender, EventArgs e)
        {

        }

        private void ClickFuturesPriceDeleteButton(object sender, EventArgs e)
        {
            StockDB.日期貨盤後行情表_luann.RemoveRange(StockDB.日期貨盤後行情表_luann);
            StockDB.SaveChanges();
        }

        private void ClickFundStatisticAddButton(object sender, EventArgs e)
        {

        }

        private void ClickFundStatisticDeleteButton(object sender, EventArgs e)
        {
            StockDB.基金非營業日統計_luann.RemoveRange(StockDB.基金非營業日統計_luann);
            StockDB.SaveChanges();
        }

        private void ClickFuturesStatisticAddButton(object sender, EventArgs e)
        {

        }

        private void ClickFuturesStatisticDeleteButton(object sender, EventArgs e)
        {
            StockDB.日期貨盤後統計表_luann.RemoveRange(StockDB.日期貨盤後統計表_luann);
            StockDB.SaveChanges();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            TimeText.Text = webBrowser1.DocumentText;
        }
    }
}
