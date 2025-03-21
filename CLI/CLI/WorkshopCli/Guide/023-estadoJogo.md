[color=white]
O jogo está quase completo, mas ainda não possui elementos cruciais que o 
definam como um verdadeiro jogo. Por exemplo, atualmente o jogador é invencível, 
o que pode ser divertido inicialmente, mas rapidamente perde a graça. Para 
adicionar mais desafio e interação, vamos implementar um sistema de Game Over!

1. Para isso, adiciona no fim da função "load()":
   [/color] [color=blue]
   -- Variáveis do Jogo
   vidas = 3
   pontuacao = 0
   estadoJogo = "a jogar" -- outros estados: "perder", "vitoria"
   [/color] [color=white]
2. Agora, adiciona no fim da função "update(dt)" o seguinte código:
   [/color] [color=blue]
    if estadoJogo == "a jogar" then
	-- Adiciona o código do update(dt) aqui
    end
   [/color] [color=white]
Coloca todo o código que tinhas anteriormente na função 
update(dt) dentro desse bloco if. Isso garante que o código só será executado 
se o jogo estiver no estado "a jogar".

3. Adiciona à função "draw(dt)" o seguinte:
   [/color] [color=blue]
   if estadoJogo == "perder" then
        love.graphics.setBackgroundColor(0.8, 0, 0) -- vermelho para Game Over
        love.graphics.print("Game Over! Pressione R para reiniciar.", love.graphics.getWidth() / 2 - 150,
            love.graphics.getHeight() / 2, 0, 1.5, 1.5)
    elseif estadoJogo == "vitoria" then
        love.graphics.setBackgroundColor(0, 0.8, 0) -- verde para Vitória
        love.graphics.print("Você venceu! Pressione R para reiniciar.", love.graphics.getWidth() / 2 - 150,
        love.graphics.getHeight() / 2, 0, 1.5, 1.5)
    else
      love.graphics.setBackgroundColor(0, 0, 0) -- fundo preto para o jogo
   
   end
   [/color] [color=white]
E adiciona tudo o que está dentro da função "draw(dt)" debaixo da linha 
"love.graphics.setBackgroundColor(0, 0, 0)".

Este código vai saber se estás a jogar, se ganhaste ou perdeste, mas ainda 
não faz nada.
[/color]
