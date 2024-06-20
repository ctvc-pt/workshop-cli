
Já tens o jogador criado e o próximo passo é fazer a movimentação da nossa nave.
Vamos começar com a movimentação para a direita.

Na nossa função "update()" adiciona o seguinte código:
<span style="color:cyan">if love.keyboard.isDown("right") then
        posicaoX = posicaoX + 1
    end
</span>

Agora corre o código usando 'Alt+L' e pressiona na tecla da seta para a direita

<span>
</span>

<pre style="color: red">
.____  _____ ____    _    _____ ___ ___  
|  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
| | | |  _| \___ \ / _ \ | |_   | | | | |
| |_| | |___ ___) / ___ \|  _|  | | |_| |
|____/|_____|____/_/   \_\_|   |___\___/
</pre>



Visto que já conseguimos fazer com que o jogador ande para o lado direito, desafio-te para o fazeres andar para o restos dos lados.

Dicas:
<span style="color:orange">esquerda-> left
baixo -> down
cima -> up</spAN>

Não te esqueças de verificar em que direção o jogador é movimentado, se for horizontal usa o posicaoX, se for vertical usa o posicaoY.

Testa o jogo(Alt+L).

<span>
</span>

<pre style="color: red">
._______  _______ ____      _    
 | ____\ \/ /_   _|  _ \    / \   
 |  _|  \  /  | | | |_) |  / _ \  
 | |___ /  \  | | |  _ .  / ___ \ 
 |_____/_/\_\ |_| |_| \_\/_/   \_\
                                  
</pre>

Se reparares a nave sai fora do ecrã, para impedir que a nave saia retira o codigo de cima e usa o seguinte codigo:

Para ter o limite para a direita:
<span style="color:cyan">if posicaoX < (love.graphics.getWidth() - imagem:getWidth()) then 
        if love.keyboard.isDown("right") then
            posicaoX = posicaoX + 1
        end
    end

Para ter o limite para a esquerda:

<span style="color:cyan">if posicaoX > 0 then 
        if love.keyboard.isDown("left") then
            posicaoX = posicaoX - 1
        end
    end

Para os sentido vertical, tens este codigo nã completo. Descobre e usa isto!

Para ter o limite para baixo. <span style="color:cyan">posicaoY &lt; (love.graphics.getHeight() - imagem:getHeight())"</span>
Para ter o limite para cima<span style="color:cyan">posicaoY > 0
