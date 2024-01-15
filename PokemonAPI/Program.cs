
using PokemonAPI.Models;

namespace PokemonAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();



            var pokemons = new List<Pokemon> 
            {
                new Pokemon { Id = 1, Name = "Bulbasaur", Type = "Grass"},
                new Pokemon { Id = 2, Name = "Ivysayr", Type = "Grass"},
                new Pokemon { Id = 3, Name = "Venosaur", Type = "Grass"},
                new Pokemon { Id = 4, Name = "Charmander", Type = "Fire"}
            };

            app.MapGet("/pokemons", () =>
            {
                return Results.Ok(pokemons);
            });

            app.MapGet("/pokemon/{id}", (int id) =>
            {
                var pokemon = pokemons.Find(p => p.Id == id);

                if (pokemon == null)
                {
                    return Results.NotFound("Sorry, this pokemon does not exist");
                }
                
                    return Results.Ok(pokemon);

            });

            app.MapPost("/pokemon", (Pokemon pokemon) =>
            {
                pokemons.Add(pokemon);
                return Results.Ok("Added to the list sucessfully");
            });


            app.MapPut("/pokemon/{id}", (int id, Pokemon pokemon) =>
            {
                var pokemonToUpdate = pokemons.Find(p => p.Id == id);
                if(pokemonToUpdate == null)
                {
                    return Results.NotFound("Sorry a pokemon by this ID does not exist");
                }

                pokemonToUpdate.Name = pokemon.Name;
                pokemonToUpdate.Type = pokemon.Type;

                return Results.Ok("Updated Pokemon succesfully!");

            });


            app.MapDelete("/pokemon/{id}", (int id) =>
            {
                var pokemonToRemove  = pokemons.Find(p => p.Id == id);
                if(pokemonToRemove == null)
                {
                    return Results.NotFound("Sorry, there is no pokemon at this id to remove");
                }

                pokemons.Remove(pokemonToRemove);
                return Results.Ok("Pokemon was removed successfully"    );
            });


            app.Run();
        }
    }
}