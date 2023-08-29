using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace assignment2_LEJ.ViewModels
{
    public class RelayCommand : ICommand
    {
        #region [필드]
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;
        #endregion

        #region [생성자]
        /**
        * @brief 파라미터가 있는 오버로드 생성자
        * @param execute 명령을 실행할 때 수행할 액션
        * @param canExecute 명령을 실행할 수 있는지 여부를 판단하는 조건. 기본값은 null
        * @return 없음
        * @note Patch-notes
        * 2023-08-11|이은진|파라미터가 있는 오버로드 생성자 
        */
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }
        /**
        * @brief 파라미터가 없는 오버로드 생성자
        * @param execute 명령을 실행할 때 수행할 액션
        * @param canExecute 명령을 실행할 수 있는지 여부를 판단하는 조건. 기본값은 null
        * @return 없음
        * @note Patch-notes
        * 2023-08-11|이은진|파라미터가 없는 오버로드 생성자 
        */
        public RelayCommand(Action execute, Predicate<object> canExecute = null)
            : this(o => execute(), canExecute)
        {
        }
        #endregion

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region [public 메서드]
        /**
        * @brief 명령을 실행할 수 있는지 여부를 확인하는 메서드
        * @param parameter 명령을 실행할 때 필요한 파라미터
        * @return 명령을 실행할 수 있는지 여부를 나타내는 bool 값
        * @note Patch-notes
        * 날짜|작성자|설명
        * 2023-08-11|이은진|명령을 실행할 수 있는지 여부를 확인하는 메서드 
        */
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }
        /**
        * @brief 명령을 실행하는 메서드
        * @param parameter 명령을 실행할 때 필요한 파라미터
        * @return 없음
        * @note Patch-notes
        * 날짜|작성자|설명
        * 2023-08-11|이은진|명령을 실행 
        */
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion
    }
}
