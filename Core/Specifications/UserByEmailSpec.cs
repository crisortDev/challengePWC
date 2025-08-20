using Challenge.Core.Abstraction;
using Challenge.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Challenge.Core.Domain.Specifications
{
    public class UserByEmailSpec : ISpecification<User>
    {
        public string Email { get; }

        public UserByEmailSpec(string email)
        {
            Email = email;
        }

        // Criterio de la consulta, filtra por email
        public Expression<Func<User, bool>> Criteria => u => u.Email == Email;

        // Aquí devolvemos una lista vacía porque no necesitamos incluir propiedades adicionales
        public List<Expression<Func<User, object>>> Includes => new List<Expression<Func<User, object>>>();
    }
}
