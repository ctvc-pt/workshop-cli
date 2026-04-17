[color=white]
Primeiro, e importante que compreendas alguns conceitos base de
programacao:

A janela do lado direito e onde vamos escrever o codigo para programar
o teu jogo.

O codigo e composto por funcoes, ou seja, um conjunto de instrucoes que
executam uma tarefa especifica. Uma funcao comeca com a palavra "function"
e termina com "end". O "end" marca o fim da funcao, para o codigo saber onde
termina.

Uma variavel e como uma mochila onde podes guardar algo importante para
o teu jogo, pode ser um numero, um texto ou ate uma caracteristica.
Depois, podes usar ou mudar ao longo do codigo.

No teu codigo ja tens 3 funcoes:

- "love.load()" - carrega tudo quando o jogo comeca
- "love.update(dt)" - atualiza o jogo muitas vezes por segundo
- "love.draw()" - desenha tudo no ecra

Por exemplo, o codigo abaixo na funcao "love.draw" escreve
texto no ecra:

[/color] [color=blue]
function love.draw()
    love.graphics.print("Ola mundo!", 100, 100)
end
[/color] [color=white]
Tudo o que aparecer a azul e o que deves copiar ou como deve aparecer
no teu codigo

[/color] [color=red]

.____  _____ ____    _    _____ ___ ___  
|  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
| | | |  _| \___ \ / _ \ | |_   | | | | |
| |_| | |___ ___) / ___ \|  _|  | | |_| |
|____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Agora quero que executas o teu jogo, carrega nas teclas 'Alt' e 'L',
e ve o que acontece.
[/color]

