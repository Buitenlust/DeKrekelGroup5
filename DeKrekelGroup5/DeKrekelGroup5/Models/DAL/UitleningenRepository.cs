using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
	public class UitleningenRepository:IUitleningenRepository
	{
        private KrekelContext context; 

	    public UitleningenRepository(KrekelContext context)
	    {
	        this.context = context; 
	    }

	    public IQueryable<Uitlening> FindAll()
	    {
	        return context.Uitleningen;
	    }

	    public Uitlening FindById(int id)
	    {
	        return context.Uitleningen.Find(id);
	    }

	    public void Add(Uitlening uitlening)
	    {
	        context.Uitleningen.Add(uitlening);
	    }

	    public void SaveChanges()
	    {
	        context.SaveChanges();
	    }
	}
}