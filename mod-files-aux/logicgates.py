import random

and_gate = lambda a, b: a and b
or_gate = lambda a, b: a or b
xor_gate = lambda a, b: a ^ b
nand_gate = lambda a, b: not (a and b)
not_gate = lambda a: not a

def apply_not_with_probability(input, probability):
    if random.random() < probability:
        return not_gate(input), True
    return input, False

def select_logic_gate():
    return random.choice([
        (and_gate, "AND"),
        (or_gate, "OR"),
        (xor_gate, "XOR"),
        (nand_gate, "NAND")
    ])

def logic_gates_module(inputs):
    diagram = ""

    # Step 1
    for i, input in enumerate(inputs):
        inputs[i], not_applied = apply_not_with_probability(input, 0.3)
        if not_applied:
            diagram += f"Input {i+1} went through a NOT.\n"

    # Step 2
    selected_input = random.choice([0, 1])
    selected_inputs = [selected_input, selected_input + 1]
    logic_gate, logic_gate_str = select_logic_gate()
    logic_gate_output = logic_gate(inputs[selected_inputs[0]], inputs[selected_inputs[1]])
    diagram += f"Inputs {selected_inputs[0]+1} and {selected_inputs[1]+1} went through a {logic_gate_str}.\n"
    logic_gate_output, not_applied = apply_not_with_probability(logic_gate_output, 0.1)
    if not_applied:
        diagram += "The output of the logic gate went through a NOT.\n"

    # Step 3
    remaining_input = list(set(range(3)) - set(selected_inputs))[0]
    inputs[remaining_input], not_applied = apply_not_with_probability(inputs[remaining_input], 0.05)
    if not_applied:
        diagram += f"Input {remaining_input+1} went through a NOT.\n"

    # Step 4
    logic_gate, logic_gate_str = select_logic_gate()
    final_output = logic_gate(logic_gate_output, inputs[remaining_input])
    diagram += f"The output of the logic gate and input {remaining_input+1} went through a {logic_gate_str}.\n"

    # Step 5
    final_output, not_applied = apply_not_with_probability(final_output, 0.5)
    if not_applied:
        diagram += "The final output went through a NOT.\n"

    return final_output, diagram

# Test
inputs = random.choices([True, False], k=3)
print(f"Inputs: {inputs}")
output, diagram = logic_gates_module(inputs)
print(f"Output: {output}")
print("Circuit diagram:")
print(diagram)