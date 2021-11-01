﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication7
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }

        }

        void BindGrid()
        {
            using (studentEntities context = new studentEntities())
            {
                if (context.Employees.Count() > 0)
                {
                    GVEmployee.DataSource = (from em in context.Employees
                                             select new { em.Emp_ID, em.Name, em.Designation, em.City, em.Country, em.State }).ToList();
                    GVEmployee.DataBind();
                }
                else
                {
                    GVEmployee.DataSource = null;
                    GVEmployee.DataBind();
                }
            }
        }

        protected void GVEmployee_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVEmployee.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void GVEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "InsertNew")
            {
                GridViewRow row = GVEmployee.FooterRow;

                TextBox txtName = row.FindControl("txtEmpNameNew") as TextBox;
                TextBox txtDesignation = row.FindControl("txtDesignationNew") as TextBox;
                TextBox txtCity = row.FindControl("txtCityNew") as TextBox;
                TextBox txtState = row.FindControl("txtStateNew") as TextBox;
                TextBox txtCountry = row.FindControl("txtCountryNew") as TextBox;

                using (studentEntities context = new studentEntities())
                {
                    Employee obj = new Employee();
                    obj.Name = txtName.Text;
                    obj.Designation = txtDesignation.Text;
                    obj.City = txtCity.Text;
                    obj.State = txtState.Text;
                    obj.Country = txtCountry.Text;
                    context.Employees.Add(obj);
                    context.SaveChanges();
                    BindGrid();
                }

            }
        }
        protected void GVEmployee_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVEmployee.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void GVEmployee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GVEmployee.EditIndex = -1;
            BindGrid();
        }
        protected void GVEmployee_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GVEmployee.Rows[e.RowIndex];

            TextBox txtName = row.FindControl("txtEmpName") as TextBox;
            TextBox txtDesignation = row.FindControl("txtDesignation") as TextBox;
            TextBox txtCity = row.FindControl("txtCity") as TextBox;
            TextBox txtState = row.FindControl("txtState") as TextBox;
            TextBox txtCountry = row.FindControl("txtCountry") as TextBox;


            using (studentEntities context = new studentEntities())
            {
                int employeeID = Convert.ToInt32(GVEmployee.DataKeys[e.RowIndex].Value);
                Employee obj = context.Employees.First(x => x.Emp_ID == employeeID);
                obj.Name = txtName.Text;
                obj.Designation = txtDesignation.Text;
                obj.City = txtCity.Text;
                obj.State = txtState.Text;
                obj.Country = txtCountry.Text;
                context.SaveChanges();

                GVEmployee.EditIndex = -1;
                BindGrid();
            }

        }

        protected void GVEmployee_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int employeeID = Convert.ToInt32(GVEmployee.DataKeys[e.RowIndex].Value);
            using (studentEntities context = new studentEntities())
            {
                Employee obj = context.Employees.First(x => x.Emp_ID == employeeID);
                context.Employees.Remove(obj);
                context.SaveChanges();
                BindGrid();
            }
        }
    }
}