[color=white]
Agora, vamos adicionar a lógica para verificar se o pássaro colidiu com os canos. 
A colisão ocorre quando o pássaro ultrapassa a área dos canos.

1. Na função "load()", adiciona as seguintes linhas de código que vão ser os sons quando o 
passaro tocar no cano:
   [/color] [color=blue]
   somColisao = love.audio.newSource("hit.wav", "static")
   somGameOver = love.audio.newSource("die.wav", "static")
   [/color] [color=white]
2. Na função "update(dt)", adiciona o seguinte código para verificar a colisão:
   [/color] [color=blue]
   for i, cano in ipairs(canos) do
     if passaro.x + passaro.largura > cano.x and passaro.x < cano.x + larguraCano then
         if passaro.y < cano.y or passaro.y + passaro.altura > cano.y + espacoCano then
               gameOver = true
               somColisao:play()
               somGameOver:play()
           end
       end
   end

   [/color] [color=white]
Este código vai verificar se o pássaro está dentro da área dos canos e, 
se estiver, verificar se há colisão com os canos superior ou inferior e vai tocar 
o som da colisão com os canos.

Para que o jogo acabe temos que adicionar a tela de "Game Over".

3. No fim da função "draw()" adiciona:
   [/color] [color=blue]
       if gameOver then
           love.graphics.printf("Game Over!", 0, love.graphics.getHeight() / 2 - 20, 
            love.graphics.getWidth(), "center")
        end
    [/color] [color=white]

4. Agora dentro da função "update(dt)", no inicio, adiciona esta linha:

   [/color] [color=blue]
   if gameOver then
       return
   end
   [/color] [color=white]

Agora quando tocares em um cano vai dar a tela de "Game Over" e o jogo acaba.
[/color] 