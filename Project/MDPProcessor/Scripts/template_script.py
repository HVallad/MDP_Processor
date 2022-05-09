import mdptoolbox
import pandas as pd
import numpy as np
import datetime
import sys
import argparse
from pathlib import Path

parser = argparse.ArgumentParser()

parser.add_argument("-tf", "--transaction", dest="transactions", type=Path)
parser.add_argument("-rf", "--rewards", dest="rewards",type=Path)
parser.add_argument("-of", "--output", dest="outputFile",type=Path)
parser.add_argument("-di", "--discount", dest="discount", type=float)
parser.add_argument("-it", "--iteration", dest="iteration", type=int)

params = parser.parse_args()
transition_filePath = params.transactions
rewards_filePath = params.rewards
output_filePath = params.outputFile
DISCOUNT_FACTOR = float(params.discount)
ITERATIONS = int(params.iteration)

transition_excel = pd.ExcelFile(transition_filePath)
rewards_excel = pd.ExcelFile(rewards_filePath)
def Load3DMatrixFromSheet(excelFile):
  worksheets = excelFile.sheet_names
  lengthSpace = -1
  multiDimensionalMatrix = np.array([])
  for sheet in worksheets:
    tempMatrix = pd.DataFrame(pd.read_excel(excelFile, sheet))
    lengthSpace = len(tempMatrix)
    multiDimensionalMatrix = np.append(multiDimensionalMatrix, tempMatrix.to_numpy())
  return multiDimensionalMatrix.reshape(-1, lengthSpace, lengthSpace)
transition_values = Load3DMatrixFromSheet(transition_excel)
reward_values = Load3DMatrixFromSheet(rewards_excel)
if len(transition_values) != len(reward_values):
    reward_values = reward_values[0]
mdp_simulator = mdptoolbox.mdp.FiniteHorizon(transition_values, reward_values, DISCOUNT_FACTOR, ITERATIONS)
mdp_simulator.run()
policy_output = pd.DataFrame(mdp_simulator.policy)
policy_output.to_excel(output_filePath, index=False)