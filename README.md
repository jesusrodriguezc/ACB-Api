Pasos a seguir para usar la API:
1) Compilar la solución. En su defecto, está el compilado en Releases>v0.0.1.
2) Lanzar el ejecutable ACB-Api.exe.
3) Ejecutar una petición POST a la URL localhost:5000/acb-api/token (En mi caso, la realicé con Postman). Del resultado, extraemos el token que servirá para autenticarnos para las llamadas posteriores.
![imagen](https://github.com/user-attachments/assets/32099e8b-4719-464b-bf7a-8322d647b81b)

4) Ejecutar la petición GET que se quiera probar, añadiendo la key 'Authorization' en la cabecera de la llamada, con valor 'Bearer <token>'.
  - GET localhost:5000/acb-api/pbp-lean/<game_id>
  - GET localhost:5000/acb-api/game-leaders/<game_id>
  - GET localhost:5000/acb-api/game-biggest_lead/<game_id>

  ![imagen](https://github.com/user-attachments/assets/bf658150-ede0-4afb-9bee-606fb1ebce63)


Toda la documentación es accesible desde localhost:5001/swagger/index.html



